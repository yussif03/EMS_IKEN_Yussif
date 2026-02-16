using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.EmployeeHandlers
{
    public class DeleteEmployeeHandler
    {
        private readonly IRepository<Employee> _repo;
        private readonly ILogger<DeleteEmployeeHandler> _logger;

        public DeleteEmployeeHandler(IRepository<Employee> repo,ILogger<DeleteEmployeeHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<bool> HandleAsync(int id)
        {
            _logger.LogInformation("Soft deleting employee with ID {EmployeeId}", id);

            var deleted = await _repo.SoftDeleteAsync(id);

            if (!deleted)
            {
                _logger.LogWarning("Delete failed. Employee with ID {EmployeeId} not found", id);
                return false;
            }

            _logger.LogInformation("Employee with ID {EmployeeId} soft deleted successfully", id);

            return true;
        }
    }
}
