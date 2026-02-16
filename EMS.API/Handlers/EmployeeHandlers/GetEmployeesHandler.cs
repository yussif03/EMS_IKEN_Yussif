using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.EmployeeHandlers
{
    public class GetEmployeesHandler
    {
        private readonly IRepository<Employee> _repo;
        private readonly ILogger<GetEmployeesHandler> _logger;

        public GetEmployeesHandler(IRepository<Employee> repo,ILogger<GetEmployeesHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<Employee>> HandleAsync()
        {
            _logger.LogInformation("Fetching all employees");

            var employees = await _repo.GetAllAsync();

            _logger.LogInformation("Returned {Count} employees", employees.Count());

            return employees;
        }
    }
}
