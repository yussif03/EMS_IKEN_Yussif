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
            })
            .WithTags("Employee")
            .WithDescription("Get all employees");

            // GET EMPLOYEE BY ID
            group.MapGet("/{id:int}", async (int id, IRepository<Employee> repo) =>
            {
                var employee = await repo.GetByIdAsync(id);

                return employee is null
                    ? Results.NotFound($"Employee with ID {id} not found")
                    : Results.Ok(employee);
            })
            .WithTags("Employee")
            .WithDescription("Get employee by id");

            // CREATE EMPLOYEE
            group.MapPost("/", async (CreateEmployeeDto dto, IRepository<Employee> repo) =>
            {
                var employee = new Employee
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    DateOfBirth = dto.DateOfBirth,
                    HireDate = dto.HireDate,
                    Address = dto.Address,
                    Salary = dto.Salary,
                    Status = dto.Status,
                    DepartmentId = dto.DepartmentId,
                    PositionId = dto.PositionId,
                    IsDeleted = false
                };

                employee.Id = await repo.AddAsync(employee);

                return Results.Created($"/api/employees/{employee.Id}", employee);
            })
            .WithTags("Employee")
            .WithDescription("Create employee");

            // UPDATE EMPLOYEE
            group.MapPut("/{id:int}", async (int id, UpdateEmployeeDto dto, IRepository<Employee> repo) =>
            {
                var employee = await repo.GetByIdAsync(id);
                if (employee is null)
                    return Results.NotFound($"Employee with ID {id} not found");

                employee.FullName = dto.FullName;
                employee.Email = dto.Email;
                employee.PhoneNumber = dto.PhoneNumber;
                employee.DateOfBirth = dto.DateOfBirth;
                employee.HireDate = dto.HireDate;
                employee.Address = dto.Address;
                employee.Salary = dto.Salary;
                employee.Status = dto.Status;
                employee.DepartmentId = dto.DepartmentId;
                employee.PositionId = dto.PositionId;

                await repo.UpdateAsync(employee);

                return Results.Ok(employee);
            })
            .WithTags("Employee")
            .WithDescription("Update employee");

            // DELETE EMPLOYEE (Soft Delete)
            group.MapDelete("/{id:int}", async (int id, IRepository<Employee> repo) =>
            {
                var deleted = await repo.SoftDeleteAsync(id);

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
