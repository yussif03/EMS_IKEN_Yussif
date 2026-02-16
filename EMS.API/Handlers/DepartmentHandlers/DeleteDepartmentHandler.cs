using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.DepartmentHandlers
{
    public class DeleteDepartmentHandler
    {
        private readonly IRepository<Department> _repo;
        private readonly ILogger<DeleteDepartmentHandler> _logger;

        public DeleteDepartmentHandler(IRepository<Department> repo, ILogger<DeleteDepartmentHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<bool> HandleAsync(int id)
        {
            _logger.LogInformation("Soft deleting department with ID {DepartmentId}", id);

            var deleted = await _repo.SoftDeleteAsync(id);

            if (!deleted)
            {
                _logger.LogWarning("Delete failed. Department with ID {DepartmentId} not found", id);
                return false;
            }

            _logger.LogInformation("Department with ID {DepartmentId} soft deleted successfully", id);

            return true;
        }
    }
}
