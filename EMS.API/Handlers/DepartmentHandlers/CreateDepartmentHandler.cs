using EMS.API.DTOs;
using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.DepartmentHandlers
{
    public class CreateDepartmentHandler
    {
        private readonly IRepository<Department> _repo;
        private readonly ILogger<CreateDepartmentHandler> _logger;

        public CreateDepartmentHandler(IRepository<Department> repo, ILogger<CreateDepartmentHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Department> HandleAsync(CreateDepartmentDto dto)
        {
            _logger.LogInformation("Creating department with Name {Name}", dto.Name);

            var department = new Department
            {
                Name = dto.Name,
                Description = dto.Description,
                ManagerId = dto.ManagerId,
                IsDeleted = false
            };

            department.Id = await _repo.AddAsync(department);

            _logger.LogInformation("Department created with ID {DepartmentId}", department.Id);

            return department;
        }
    }
}
