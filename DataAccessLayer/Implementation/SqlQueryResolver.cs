using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersPoint.Core;
using TeachersPoint.DataAccessLayer.Interface;

namespace TeachersPoint.DataAccessLayer.Implementation
{
    public class SqlQueryResolver : ISqlQueryResolver
    {
        private readonly IConfiguration _configuration;
        public SqlQueryResolver(IConfiguration config)
        {
            _configuration = config;
        }
        public dynamic ResolveSqlQuery(string query)
        {
            // Connection string from configuration
            string connectionString = _configuration.GetConnectionString(Constants.SqlConnectionName);

            try
            {
                // Create a DataTable to store the results
                DataTable dataTable = new DataTable();

                // Use using blocks to ensure proper disposal of resources
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Use NpgsqlDataAdapter to execute the query and fill the DataTable
                    using (var adapter = new NpgsqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable);
                    }
                }

                // Return the DataTable 
                return dataTable;
            }
            catch (Npgsql.PostgresException ex)
            {

                if (ex.SqlState == "23505") // Duplicate key error
                {
                    // Convert the exception into a meaningful error message
                    string errorMessage = "A record with the same key already exists in the database.";

                    // Optionally, you can add more details such as the table name or column name
                    // errorMessage += $" Please check the value for column {columnName}.";

                    // Pass the meaningful error message to the controller
                    
                    return errorMessage;
                }
                else
                {
                    // Re-throw other types of exceptions as they are
                    throw;
                }
            }
        }
    }
}
