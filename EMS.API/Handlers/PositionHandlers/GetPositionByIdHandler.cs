using EMS.API.Handlers.EmployeeHandlers;
using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.PositionHandlers
{
    public class GetPositionByIdHandler
    {
        private readonly IRepository<Position> _repo;
        private readonly ILogger<GetPositionByIdHandler> _logger;

        public GetPositionByIdHandler(IRepository<Position> repo, ILogger<GetPositionByIdHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Position?> HandleAsync(int id)
        {
            _logger.LogInformation("Fetching position with ID {PositionId}", id);

            var position = await _repo.GetByIdAsync(id);

            if (position is null)
            {
                _logger.LogWarning("Position with ID {PositionId} not found", id);
                return null;
            }

            _logger.LogInformation("Position with ID {PositionId} retrieved successfully", id);

            return position;
        }
    }
}
