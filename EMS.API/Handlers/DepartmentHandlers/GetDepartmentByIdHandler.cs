using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.DepartmentHandlers
{
    public class GetDepartmentByIdHandler
    {
        private readonly IRepository<Department> _repo;
        private readonly ILogger<GetDepartmentByIdHandler> _logger;

        public GetDepartmentByIdHandler(IRepository<Department> repo, ILogger<GetDepartmentByIdHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Department?> HandleAsync(int id)
        {
            _logger.LogInformation("Fetching department with ID {DepartmentId}", id);

            var department = await _repo.GetByIdAsync(id);

            if (department is null)
            {
                _logger.LogWarning("Department with ID {DepartmentId} not found", id);
                return null;
            }

            _logger.LogInformation("Department with ID {DepartmentId} retrieved successfully", id);

            return department;
        }
    }
}
