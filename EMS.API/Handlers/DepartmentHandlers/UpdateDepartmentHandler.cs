using EMS.API.Models;
using EMS.API.Models.DTOs;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.DepartmentHandlers
{
    public class UpdateDepartmentHandler
    {
        private readonly IRepository<Department> _repo;
        private readonly ILogger<UpdateDepartmentHandler> _logger;

        public UpdateDepartmentHandler(IRepository<Department> repo, ILogger<UpdateDepartmentHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Department?> HandleAsync(int id, UpdateDepartmentDto dto)
        {
            _logger.LogInformation("Updating department with ID {DepartmentId}", id);

            var department = await _repo.GetByIdAsync(id);

            if (department is null)
            {
                _logger.LogWarning("Update failed. Department with ID {DepartmentId} not found", id);
                return null;
            }

            department.Name = dto.Name;
            department.Description = dto.Description;
            department.ManagerId = dto.ManagerId;

            await _repo.UpdateAsync(department);

            _logger.LogInformation("Department with ID {DepartmentId} updated successfully", id);

            return department;
        }
    }
}
