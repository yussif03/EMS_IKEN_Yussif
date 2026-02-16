using EMS.API.DTOs;
using EMS.API.Handlers.EmployeeHandlers;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints
{
    public static class EmployeeEndpoints
    {
        public static RouteGroupBuilder MapEmployeeEndpoints(this RouteGroupBuilder group)
        {
            // GET ALL EMPLOYEES
            group.MapGet("/", async ([FromServices]GetEmployeesHandler handler) =>
            {
                var employees = await handler.HandleAsync();
                return Results.Ok(employees);
            })
            .WithTags("Employee")
            .WithDescription("Get all employees");


            // GET EMPLOYEE BY ID
            group.MapGet("/{id:int}", async (int id, [FromServices]GetEmployeeByIdHandler handler) =>
            {
                var employee = await handler.HandleAsync(id);

                return employee is null
                    ? Results.NotFound($"Employee with ID {id} not found")
                    : Results.Ok(employee);
            })
            .WithTags("Employee")
            .WithDescription("Get employee by id");


            // CREATE EMPLOYEE
            group.MapPost("/", async ([FromBody]CreateEmployeeDto dto, [FromServices]CreateEmployeeHandler handler) =>
            {
                var employee = await handler.HandleAsync(dto);

                return Results.Created(
                    $"/api/employees/{employee.Id}",
                    employee);
            })
            .WithTags("Employee")
            .WithDescription("Create employee");


            // UPDATE EMPLOYEE
            group.MapPut("/{id:int}", async (int id, [FromBody]UpdateEmployeeDto dto, [FromServices]UpdateEmployeeHandler handler) =>
            {
                var updated = await handler.HandleAsync(id, dto);

                return updated is null
                    ? Results.NotFound($"Employee with ID {id} not found")
                    : Results.Ok(updated);
            })
            .WithTags("Employee")
            .WithDescription("Update employee");


            // DELETE EMPLOYEE
            group.MapDelete("/{id:int}", async (int id, [FromServices]DeleteEmployeeHandler handler) =>
            {
                var deleted = await handler.HandleAsync(id);

                return deleted
                    ? Results.NoContent()
                    : Results.NotFound($"Employee with ID {id} not found");
            })
            .WithTags("Employee")
            .WithDescription("Delete employee");

            return group;
        }
    }
}
