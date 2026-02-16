using EMS.API.Models;
using EMS.API.Models.DTOs;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.EmployeeHandlers
{
    public class CreateEmployeeHandler
    {
        private readonly IRepository<Employee> _repo;
        private readonly ILogger<CreateEmployeeHandler> _logger;

        public CreateEmployeeHandler(IRepository<Employee> repo, ILogger<CreateEmployeeHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Employee> HandleAsync(CreateEmployeeDto dto)
        {
            _logger.LogInformation("Creating employee with Email {Email}", dto.Email);

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

            employee.Id = await _repo.AddAsync(employee);

            _logger.LogInformation("Employee created with ID {EmployeeId}", employee.Id);

            return employee;
        }
    }
}
