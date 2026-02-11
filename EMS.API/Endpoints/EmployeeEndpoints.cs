using EMS.API.Models;
using EMS.API.Models.DTOs;
using EMS.API.Repositories.Base;

namespace EMS.API.Endpoints
{
    public static class EmployeeEndpoints
    {
        public static RouteGroupBuilder MapEmployeeEndpoints(this RouteGroupBuilder group)
        {
            // GET ALL EMPLOYEES
            group.MapGet("/", async (IRepository<Employee> repo, ILogger<EmployeeRepository> logger) =>
            {
                logger.LogInformation("Fetching all employees");

                var employees = await repo.GetAllAsync();

                logger.LogInformation("Returned {Count} employees", employees.Count());

                return Results.Ok(employees);
            }).WithTags("Employee").WithDescription("Get all employees");

            // GET EMPLOYEE BY ID
            group.MapGet("/{id:int}", async (int id, IRepository<Employee> repo, ILogger<EmployeeRepository> logger) =>
            {
                logger.LogInformation("Fetching employee with ID {EmployeeId}", id);

                var employee = await repo.GetByIdAsync(id);

                if (employee is null)
                {
                    logger.LogWarning("Employee with ID {EmployeeId} not found", id);
                    return Results.NotFound($"Employee with ID {id} not found");
                }

                logger.LogInformation("Employee with ID {EmployeeId} retrieved successfully", id);
                return Results.Ok(employee);
            }).WithTags("Employee").WithDescription("Get employee by id");

            // CREATE EMPLOYEE
            group.MapPost("/", async (CreateEmployeeDto dto, IRepository<Employee> repo, ILogger<EmployeeRepository> logger) =>
            {
                logger.LogInformation("Creating new employee with email {Email}", dto.Email);

                var employee = new Employee
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    HireDate = dto.HireDate,
                    Salary = dto.Salary,
                    Status = dto.Status,
                    DepartmentId = dto.DepartmentId,
                    PositionId = dto.PositionId,
                    IsDeleted = false
                };

                var newId = await repo.AddAsync(employee);
                employee.Id = newId;

                logger.LogInformation("Employee created with ID {EmployeeId}", newId);

                return Results.Created($"/api/employees/{newId}", employee);
            }).WithTags("Employee").WithDescription("Create employee");

            // UPDATE EMPLOYEE
            group.MapPut("/{id:int}", async (int id, UpdateEmployeeDto dto, IRepository<Employee> repo, ILogger<EmployeeRepository> logger) =>
            {
                logger.LogInformation("Updating employee with ID {EmployeeId}", id);

                var existingEmployee = await repo.GetByIdAsync(id);
                if (existingEmployee is null)
                {
                    logger.LogWarning("Update failed. Employee with ID {EmployeeId} not found", id);
                    return Results.NotFound($"Employee with ID {id} not found");
                }

                existingEmployee.FullName = dto.FullName;
                existingEmployee.Email = dto.Email;
                existingEmployee.PhoneNumber = dto.PhoneNumber;
                existingEmployee.HireDate = dto.HireDate;
                existingEmployee.Salary = dto.Salary;
                existingEmployee.Status = dto.Status;
                existingEmployee.DepartmentId = dto.DepartmentId;
                existingEmployee.PositionId = dto.PositionId;

                var updated = await repo.UpdateAsync(existingEmployee);

                if (!updated)
                {
                    logger.LogWarning("Update failed in repository for Employee ID {EmployeeId}", id);
                    return Results.NotFound($"Employee with ID {id} not found");
                }

                logger.LogInformation("Employee with ID {EmployeeId} updated successfully", id);
                return Results.Ok(existingEmployee);
            }).WithTags("Employee").WithDescription("Update employee");

            // DELETE EMPLOYEE (Soft Delete)
            group.MapDelete("/{id:int}", async (int id, IRepository<Employee> repo, ILogger<EmployeeRepository> logger) =>
            {
                logger.LogInformation("Soft deleting employee with ID {EmployeeId}", id);

                var deleted = await repo.SoftDeleteAsync(id);

                if (!deleted)
                {
                    logger.LogWarning("Soft delete failed. Employee with ID {EmployeeId} not found", id);
                    return Results.NotFound($"Employee with ID {id} not found");
                }

                logger.LogInformation("Employee with ID {EmployeeId} soft deleted successfully", id);
                return Results.NoContent();
            }).WithTags("Employee").WithDescription("Delete employee");

            return group;
        }
    }
}
