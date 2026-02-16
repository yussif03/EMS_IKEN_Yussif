using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.DepartmentHandlers
{
    public class GetDepartmentsHandler
    {
        private readonly IRepository<Department> _repo;
        private readonly ILogger<GetDepartmentsHandler> _logger;

        public GetDepartmentsHandler(IRepository<Department> repo, ILogger<GetDepartmentsHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<Department>> HandleAsync()
        {
            _logger.LogInformation("Fetching all departments");

            var departments = await _repo.GetAllAsync();

            _logger.LogInformation("Returned {Count} departments", departments.Count());

            return departments;
        }
    }
}
