using EMS.API.DTOs;
using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.EmployeeHandlers
{
    public class UpdateEmployeeHandler
    {
        private readonly IRepository<Employee> _repo;
        private readonly ILogger<UpdateEmployeeHandler> _logger;

        public UpdateEmployeeHandler(IRepository<Employee> repo, ILogger<UpdateEmployeeHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Employee?> HandleAsync(int id, UpdateEmployeeDto dto)
        {
            _logger.LogInformation("Updating employee with ID {EmployeeId}", id);

            var employee = await _repo.GetByIdAsync(id);

            if (employee is null)
            {
                _logger.LogWarning("Update failed. Employee with ID {EmployeeId} not found", id);
                return null;
            }

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

            await _repo.UpdateAsync(employee);

            _logger.LogInformation("Employee with ID {EmployeeId} updated successfully", id);

            return employee;
        }
    }
}
