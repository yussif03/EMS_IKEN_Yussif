using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.EmployeeHandlers
{
    public class GetEmployeeByIdHandler
    {
        private readonly IRepository<Employee> _repo;
        private readonly ILogger<GetEmployeeByIdHandler> _logger;

        public GetEmployeeByIdHandler(IRepository<Employee> repo, ILogger<GetEmployeeByIdHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Employee?> HandleAsync(int id)
        {
            _logger.LogInformation("Fetching employee with ID {EmployeeId}", id);

            var employee = await _repo.GetByIdAsync(id);

            if (employee is null)
            {
                _logger.LogWarning("Employee with ID {EmployeeId} not found", id);
                return null;
            }

            _logger.LogInformation("Employee with ID {EmployeeId} retrieved successfully", id);

            return employee;
        }
    }
}
