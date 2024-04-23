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
        public DataTable ResolveSqlQuery(string query)
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
            catch
            {

                throw;
            }
        }
    }
}
