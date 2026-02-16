using EMS.API.Models;
using EMS.API.Models.DTOs;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.PositionHandlers
{
    public class UpdatePositionHandler
    {
        private readonly IRepository<Position> _repo;
        private readonly ILogger<UpdatePositionHandler> _logger;

        public UpdatePositionHandler(IRepository<Position> repo, ILogger<UpdatePositionHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Position?> HandleAsync(int id, UpdatePositionDto dto)
        {
            _logger.LogInformation("Updating position with ID {PositionId}", id);

            var position = await _repo.GetByIdAsync(id);

            if (position is null)
            {
                _logger.LogWarning("Update failed. Position with ID {PositionId} not found", id);
                return null;
            }

            position.Title = dto.Title;
            position.MinSalary = dto.MinSalary;
            position.MaxSalary = dto.MaxSalary;

            await _repo.UpdateAsync(position);

            _logger.LogInformation("Position with ID {PositionId} updated successfully", id);

            return position;
        }
    }
}
