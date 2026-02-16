using EMS.API.Handlers.DepartmentHandlers;
using EMS.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints
{
    public static class DepartmentEndpoints
    {
        public static RouteGroupBuilder MapDepartmentEndpoints(this RouteGroupBuilder group)
        {
            // GET ALL DEPARTMENTS
            group.MapGet("/", async ([FromServices]GetDepartmentsHandler handler) =>
            {
                var departments = await handler.HandleAsync();
                return Results.Ok(departments);
            })
            .WithTags("Department")
            .WithDescription("Get all departments");


            // GET DEPARTMENT BY ID
            group.MapGet("/{id:int}", async (int id, [FromServices]GetDepartmentByIdHandler handler) =>
            {
                var department = await handler.HandleAsync(id);

                return department is null
                    ? Results.NotFound($"Department with ID {id} not found")
                    : Results.Ok(department);
            })
            .WithTags("Department")
            .WithDescription("Get department by id");


            // CREATE DEPARTMENT
            group.MapPost("/", async ([FromBody]CreateDepartmentDto dto, [FromServices]CreateDepartmentHandler handler) =>
            {
                var department = await handler.HandleAsync(dto);

                return Results.Created(
                    $"/api/departments/{department.Id}",
                    department);
            })
            .WithTags("Department")
            .WithDescription("Create department");


            // UPDATE DEPARTMENT
            group.MapPut("/{id:int}", async (int id, [FromBody]UpdateDepartmentDto dto, [FromServices]UpdateDepartmentHandler handler) =>
            {
                var updated = await handler.HandleAsync(id, dto);

                return updated is null
                    ? Results.NotFound($"Department with ID {id} not found")
                    : Results.Ok(updated);
            })
            .WithTags("Department")
            .WithDescription("Update department");


            // DELETE DEPARTMENT (Soft Delete)
            group.MapDelete("/{id:int}", async (int id, [FromServices]DeleteDepartmentHandler handler) =>
            {
                var deleted = await handler.HandleAsync(id);

                return deleted
                    ? Results.NoContent()
                    : Results.NotFound($"Department with ID {id} not found");
            })
            .WithTags("Department")
            .WithDescription("Delete department");

            return group;
        }
    }
}
