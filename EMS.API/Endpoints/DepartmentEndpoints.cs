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
            });

            // GET DEPARTMENT BY ID
            group.MapGet("/{id:int}", async (int id, IRepository<Department> repo) =>
            {
                var department = await repo.GetByIdAsync(id);
                return department is null
                    ? Results.NotFound($"Department with ID {id} not found")
                    : Results.Ok(department);
            });

            // CREATE DEPARTMENT
            group.MapPost("/", async (CreateDepartmentDto dto, IRepository<Department> repo) =>
            {
                var department = new Department
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    IsDeleted = false
                };

                var newId = await repo.AddAsync(department);
                department.Id = newId;

                return Results.Created($"/api/departments/{newId}", department);
            });

            // UPDATE DEPARTMENT
            group.MapPut("/{id:int}", async (int id, UpdateDepartmentDto dto, IRepository<Department> repo) =>
            {
                var existingDepartment = await repo.GetByIdAsync(id);
                if (existingDepartment is null)
                {
                    return Results.NotFound($"Department with ID {id} not found");
                }

                existingDepartment.Name = dto.Name;
                existingDepartment.Description = dto.Description;

                var updated = await repo.UpdateAsync(existingDepartment);
                return updated
                    ? Results.Ok(existingDepartment)
                    : Results.NotFound($"Department with ID {id} not found");
            });

            // DELETE DEPARTMENT (Soft Delete)
            group.MapDelete("/{id:int}", async (int id, IRepository<Department> repo) =>
            {
                var deleted = await repo.SoftDeleteAsync(id);
                return deleted
                    ? Results.NoContent()
                    : Results.NotFound($"Department with ID {id} not found");
            });

            return group;
        }
    }
}
