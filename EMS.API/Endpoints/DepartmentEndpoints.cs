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
            group.MapGet("/", async (IRepository<Department> repo, ILogger<DepartmentRepository> logger) =>
            {
                logger.LogInformation("Fetching all departments");

                var departments = await repo.GetAllAsync();

                logger.LogInformation("Returned {Count} departments", departments.Count());
                return Results.Ok(departments);
            }).WithTags("Department").WithDescription("Get all departments");

            // GET DEPARTMENT BY ID
            group.MapGet("/{id:int}", async (int id, IRepository<Department> repo, ILogger<DepartmentRepository> logger) =>
            {
                logger.LogInformation("Fetching department with ID {DepartmentId}", id);

                var department = await repo.GetByIdAsync(id);

                if (department is null)
                {
                    logger.LogWarning("Department with ID {DepartmentId} not found", id);
                    return Results.NotFound($"Department with ID {id} not found");
                }

                logger.LogInformation("Department with ID {DepartmentId} retrieved successfully", id);
                return Results.Ok(department);
            }).WithTags("Department").WithDescription("Get department by id");

            // CREATE DEPARTMENT
            group.MapPost("/", async (CreateDepartmentDto dto, IRepository<Department> repo, ILogger<DepartmentRepository> logger) =>
            {
                logger.LogInformation("Creating new department with name {DepartmentName}", dto.Name);

                var department = new Department
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    IsDeleted = false
                };

                var newId = await repo.AddAsync(department);
                department.Id = newId;

                logger.LogInformation("Department created with ID {DepartmentId}", newId);
                return Results.Created($"/api/departments/{newId}", department);
            }).WithTags("Department").WithDescription("Create department");

            // UPDATE DEPARTMENT
            group.MapPut("/{id:int}", async (int id, UpdateDepartmentDto dto, IRepository<Department> repo, ILogger<DepartmentRepository> logger) =>
            {
                logger.LogInformation("Updating department with ID {DepartmentId}", id);

                var existingDepartment = await repo.GetByIdAsync(id);
                if (existingDepartment is null)
                {
                    logger.LogWarning("Update failed. Department with ID {DepartmentId} not found", id);
                    return Results.NotFound($"Department with ID {id} not found");
                }

                existingDepartment.Name = dto.Name;
                existingDepartment.Description = dto.Description;

                var updated = await repo.UpdateAsync(existingDepartment);

                if (!updated)
                {
                    logger.LogWarning("Update failed in repository for Department ID {DepartmentId}", id);
                    return Results.NotFound($"Department with ID {id} not found");
                }

                logger.LogInformation("Department with ID {DepartmentId} updated successfully", id);
                return Results.Ok(existingDepartment);
            }).WithTags("Department").WithDescription("Update department");

            // DELETE DEPARTMENT (Soft Delete)
            group.MapDelete("/{id:int}", async (int id, IRepository<Department> repo, ILogger<DepartmentRepository> logger) =>
            {
                logger.LogInformation("Soft deleting department with ID {DepartmentId}", id);

                var deleted = await repo.SoftDeleteAsync(id);

                if (!deleted)
                {
                    logger.LogWarning("Soft delete failed. Department with ID {DepartmentId} not found", id);
                    return Results.NotFound($"Department with ID {id} not found");
                }

                logger.LogInformation("Department with ID {DepartmentId} soft deleted successfully", id);
                return Results.NoContent();
            }).WithTags("Department").WithDescription("Delete department");

            return group;
        }
    }
}
