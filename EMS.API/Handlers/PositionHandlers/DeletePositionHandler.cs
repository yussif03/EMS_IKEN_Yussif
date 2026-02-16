using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.PositionHandlers
{
    public class DeletePositionHandler
    {
        private readonly IRepository<Position> _repo;
        private readonly ILogger<DeletePositionHandler> _logger;

        public DeletePositionHandler(IRepository<Position> repo, ILogger<DeletePositionHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<bool> HandleAsync(int id)
        {
            _logger.LogInformation("Soft deleting position with ID {PositionId}", id);

            var deleted = await _repo.SoftDeleteAsync(id);

            if (!deleted)
            {
                _logger.LogWarning("Delete failed. Position with ID {PositionId} not found", id);
                return false;
            }

            _logger.LogInformation("Position with ID {PositionId} soft deleted successfully", id);

            return true;
        }
    }
}
