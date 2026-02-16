using EMS.API.Models;
using EMS.API.Models.DTOs;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.PositionHandlers
{
    public class CreatePositionHandler
    {
        private readonly IRepository<Position> _repo;
        private readonly ILogger<CreatePositionHandler> _logger;

        public CreatePositionHandler(IRepository<Position> repo, ILogger<CreatePositionHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Position> HandleAsync(CreatePositionDto dto)
        {
            _logger.LogInformation("Creating position with Title {Title}", dto.Title);

            var position = new Position
            {
                Title = dto.Title,
                MinSalary = dto.MinSalary,
                MaxSalary = dto.MaxSalary,
                IsDeleted = false
            };

            position.Id = await _repo.AddAsync(position);

            _logger.LogInformation("Position created with ID {PositionId}", position.Id);

            return position;
        }
    }
}
