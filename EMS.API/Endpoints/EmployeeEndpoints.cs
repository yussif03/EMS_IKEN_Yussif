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
            group.MapGet("/", async (IRepository<Employee> repo) =>
            {
                var employees = await repo.GetAllAsync();
                return Results.Ok(employees);
            });

            // GET EMPLOYEE BY ID
            group.MapGet("/{id:int}", async (int id, IRepository<Employee> repo) =>
            {
                var employee = await repo.GetByIdAsync(id);
                return employee is null
                    ? Results.NotFound($"Employee with ID {id} not found")
                    : Results.Ok(employee);
            });

            // CREATE EMPLOYEE
            group.MapPost("/", async (CreateEmployeeDto dto, IRepository<Employee> repo) =>
            {
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
                    isDeleted = false
                };

                var newId = await repo.AddAsync(employee);
                employee.Id = newId;

                return Results.Created($"/api/employees/{newId}", employee);
            });

            // UPDATE EMPLOYEE
            group.MapPut("/{id:int}", async (int id, UpdateEmployeeDto dto, IRepository<Employee> repo) =>
            {
                var existingEmployee = await repo.GetByIdAsync(id);
                if (existingEmployee is null)
                {
                    return Results.NotFound($"Employee with ID {id} not found");
                }

                existingEmployee.FullName = dto.FullName;
                existingEmployee.Email = dto.Email;
                existingEmployee.PhoneNumber = dto.PhoneNumber;
                existingEmployee.HireDate = dto.HireDate;
                existingEmployee.Salary = dto.Salary;
                existingEmployee.Status = dto.Status;
                existingEmployee.isDeleted = dto.isDeleted;
                existingEmployee.DepartmentId = dto.DepartmentId;
                existingEmployee.PositionId = dto.PositionId;

                var updated = await repo.UpdateAsync(existingEmployee);
                return updated
                    ? Results.Ok(existingEmployee)
                    : Results.NotFound($"Employee with ID {id} not found");
            });

            // DELETE EMPLOYEE (Soft Delete)
            group.MapDelete("/{id:int}", async (int id, IRepository<Employee> repo) =>
            {
                var deleted = await repo.SoftDeleteAsync(id);
                return deleted
                    ? Results.NoContent()
                    : Results.NotFound($"Employee with ID {id} not found");
            });

            return group;
        }
    }
}
