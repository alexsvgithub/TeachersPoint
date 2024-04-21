using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Xml.Linq;
using TeachersPoint.BusinessLayer.Implementation;
using TeachersPoint.BusinessLayer.Interface;
using TeachersPoint.Core.RequestDto;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TeachersPoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<ITestService, TestService>();

            #region Adding Service for MongoDB Connection
                //Adding Service for MongoDB Connection
                builder.Services.AddSingleton<IMongoClient>(sp =>
                new MongoClient("mongodb://localhost:27017")); // Use your MongoDB connection string

                builder.Services.AddSingleton(sp =>
                {
                    var client = sp.GetRequiredService<IMongoClient>();
                    return client.GetDatabase("TeachersPoint"); // Use your database name
                });

                builder.Services.AddSingleton(sp =>
                {
                    var database = sp.GetRequiredService<IMongoDatabase>();
                    return database.GetCollection<MongoRequestDto>("EditableDetails"); // Use your collection name
                });
            #endregion


            //// Read the connection string from configuration
            //var connectionString = ConfigurationManager.GetConnectionString("DefaultConnection");

            //// Add DbContext with PostgreSQL provider
            //builder.Services<MyAppDbContext>(options =>
            //    options.UseNpgsql(connectionString));

            #region Adding CORS
            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            #endregion

            #region Set JSON Serialization As Default
            builder.Services.AddControllersWithViews().AddNewtonsoftJson();
            #endregion



            builder.Services.AddControllers();
            


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            #region Using CORS
            app.UseCors(options=>options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}