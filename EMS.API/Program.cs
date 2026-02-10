using EMS.API.Data.Implementation;
using EMS.API.Data.Interface;
using EMS.API.Endpoints;
using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Read configuration values
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var appName = builder.Configuration["ApplicationSettings:ApplicationName"];

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

            builder.Services.AddScoped<IRepository<Employee>, EmployeeRepository>();
            builder.Services.AddScoped<IRepository<Department>, DepartmentRepository>();
            builder.Services.AddScoped<IRepository<Position>, PositionRepository>();

            var app = builder.Build();

            var logger = app.Logger;
            logger.LogInformation("Application Name: {AppName}", appName);
            logger.LogInformation("Database Connection: {ConnectionString}", connectionString);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapGroup("/api/employees").MapEmployeeEndpoints();

            app.Run();
        }
    }
}
