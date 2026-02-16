using EMS.API.Handlers.EmployeeHandlers;
using EMS.API.Models;
using EMS.API.Repositories.Base;

namespace EMS.API.Handlers.PositionHandlers
{
    public class GetPositionsHandler
    {
        private readonly IRepository<Position> _repo;
        private readonly ILogger<GetPositionsHandler> _logger;

        public GetPositionsHandler(IRepository<Position> repo, ILogger<GetPositionsHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IEnumerable<Position>> HandleAsync()
        {
            _logger.LogInformation("Fetching all positions");

            var positions = await _repo.GetAllAsync();

            _logger.LogInformation("Returned {Count} positions", positions.Count());

            return positions;
        }
    }
}
