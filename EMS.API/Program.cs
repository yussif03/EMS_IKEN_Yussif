using EMS.API.Data.Implementation;
using EMS.API.Data.Interface;
using EMS.API.Endpoints;
using EMS.API.Handlers.DepartmentHandlers;
using EMS.API.Handlers.EmployeeHandlers;
using EMS.API.Handlers.PositionHandlers;
using EMS.API.Middleware;
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

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

            builder.Services.AddScoped<IRepository<Employee>, EmployeeRepository>();
            builder.Services.AddScoped<IRepository<Department>, DepartmentRepository>();
            builder.Services.AddScoped<IRepository<Position>, PositionRepository>();

            builder.Services.AddScoped<GetEmployeesHandler>(); // scoped --> repository scoped per request
            builder.Services.AddScoped<GetEmployeeByIdHandler>();
            builder.Services.AddScoped<CreateEmployeeHandler>();
            builder.Services.AddScoped<UpdateEmployeeHandler>();
            builder.Services.AddScoped<DeleteEmployeeHandler>();

            builder.Services.AddScoped<GetDepartmentsHandler>();
            builder.Services.AddScoped<GetDepartmentByIdHandler>();
            builder.Services.AddScoped<CreateDepartmentHandler>();
            builder.Services.AddScoped<UpdateDepartmentHandler>();
            builder.Services.AddScoped<DeleteDepartmentHandler>();

            builder.Services.AddScoped<GetPositionsHandler>();
            builder.Services.AddScoped<GetPositionByIdHandler>();
            builder.Services.AddScoped<CreatePositionHandler>();
            builder.Services.AddScoped<UpdatePositionHandler>();
            builder.Services.AddScoped<DeletePositionHandler>();

            var app = builder.Build();

            var logger = app.Logger;
            logger.LogInformation("Application Name: {AppName}", appName);
            logger.LogInformation("Database Connection: {ConnectionString}", connectionString);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGroup("/api/employees").MapEmployeeEndpoints();
            app.MapGroup("/api/departments").MapDepartmentEndpoints();
            app.MapGroup("/api/positions").MapPositionEndpoints();

            app.Run();
        }
    }
}
