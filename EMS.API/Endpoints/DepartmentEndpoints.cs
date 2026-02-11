using EMS.API.Models;
using EMS.API.Models.DTOs;
using EMS.API.Repositories.Base;

namespace EMS.API.Endpoints
{
    public static class DepartmentEndpoints
    {
        public static RouteGroupBuilder MapDepartmentEndpoints(this RouteGroupBuilder group)
        {
            // GET ALL DEPARTMENTS
            group.MapGet("/", async (IRepository<Department> repo) =>
            {
                var departments = await repo.GetAllAsync();
                return Results.Ok(departments);
            })
            .WithTags("Department")
            .WithDescription("Get all departments");

            // GET DEPARTMENT BY ID
            group.MapGet("/{id:int}", async (int id, IRepository<Department> repo) =>
            {
                var department = await repo.GetByIdAsync(id);

                return department is null
                    ? Results.NotFound($"Department with ID {id} not found")
                    : Results.Ok(department);
            })
            .WithTags("Department")
            .WithDescription("Get department by id");

            // CREATE DEPARTMENT
            group.MapPost("/", async (CreateDepartmentDto dto, IRepository<Department> repo) =>
            {
                var department = new Department
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ManagerId = dto.ManagerId,
                    IsDeleted = false
                };

                department.Id = await repo.AddAsync(department);

                return Results.Created($"/api/departments/{department.Id}", department);
            })
            .WithTags("Department")
            .WithDescription("Create department");

            // UPDATE DEPARTMENT
            group.MapPut("/{id:int}", async (int id, UpdateDepartmentDto dto, IRepository<Department> repo) =>
            {
                var department = await repo.GetByIdAsync(id);
                if (department is null)
                    return Results.NotFound($"Department with ID {id} not found");

                department.Name = dto.Name;
                department.Description = dto.Description;
                department.ManagerId = dto.ManagerId;

                await repo.UpdateAsync(department);

                return Results.Ok(department);
            })
            .WithTags("Department")
            .WithDescription("Update department");

            // DELETE DEPARTMENT (Soft Delete)
            group.MapDelete("/{id:int}", async (int id, IRepository<Department> repo) =>
            {
                var deleted = await repo.SoftDeleteAsync(id);

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
