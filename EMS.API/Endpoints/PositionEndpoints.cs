using EMS.API.Models;
using EMS.API.Models.DTOs;
using EMS.API.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace EMS.API.Endpoints
{
    public static class PositionEndpoints
    {
        public static RouteGroupBuilder MapPositionEndpoints(this RouteGroupBuilder group)
        {
            // GET ALL POSITIONS
            group.MapGet("/", async (IRepository<Position> repo, ILogger<PositionRepository> logger) =>
            {
                logger.LogInformation("Fetching all positions");

                var positions = await repo.GetAllAsync();

                logger.LogInformation("Returned {Count} positions", positions.Count());
                return Results.Ok(positions);
            }).WithTags("Position").WithDescription("Get all positions");

            // GET POSITION BY ID
            group.MapGet("/{id:int}", async (int id, IRepository<Position> repo, ILogger<PositionRepository> logger) =>
            {
                logger.LogInformation("Fetching position with ID {PositionId}", id);

                var position = await repo.GetByIdAsync(id);

                if (position is null)
                {
                    logger.LogWarning("Position with ID {PositionId} not found", id);
                    return Results.NotFound($"Position with ID {id} not found");
                }

                logger.LogInformation("Position with ID {PositionId} retrieved successfully", id);
                return Results.Ok(position);
            }).WithTags("Position").WithDescription("Get position by id");

            // CREATE POSITION
            group.MapPost("/", async (CreatePositionDto dto, IRepository<Position> repo, ILogger<PositionRepository> logger) =>
            {
                logger.LogInformation("Creating new position with title {Title}", dto.Title);

                var position = new Position
                {
                    Title = dto.Title,
                    MinSalary = dto.MinSalary,
                    MaxSalary = dto.MaxSalary,
                    IsDeleted = false
                };

                var newId = await repo.AddAsync(position);
                position.Id = newId;

                logger.LogInformation("Position created with ID {PositionId}", newId);
                return Results.Created($"/api/positions/{newId}", position);
            }).WithTags("Position").WithDescription("Create position");

            // UPDATE POSITION
            group.MapPut("/{id:int}", async (int id, UpdatePositionDto dto, IRepository<Position> repo, ILogger<PositionRepository> logger) =>
            {
                logger.LogInformation("Updating position with ID {PositionId}", id);

                var existingPosition = await repo.GetByIdAsync(id);
                if (existingPosition is null)
                {
                    logger.LogWarning("Update failed. Position with ID {PositionId} not found", id);
                    return Results.NotFound($"Position with ID {id} not found");
                }

                existingPosition.Title = dto.Title;
                existingPosition.MinSalary = dto.MinSalary;
                existingPosition.MaxSalary = dto.MaxSalary;

                var updated = await repo.UpdateAsync(existingPosition);

                if (!updated)
                {
                    logger.LogWarning("Update failed in repository for Position ID {PositionId}", id);
                    return Results.NotFound($"Position with ID {id} not found");
                }

                logger.LogInformation("Position with ID {PositionId} updated successfully", id);
                return Results.Ok(existingPosition);
            }).WithTags("Position").WithDescription("Update position");

            // DELETE POSITION (Soft Delete)
            group.MapDelete("/{id:int}", async (int id, IRepository<Position> repo, ILogger<PositionRepository> logger) =>
            {
                logger.LogInformation("Soft deleting position with ID {PositionId}", id);

                var deleted = await repo.SoftDeleteAsync(id);

                if (!deleted)
                {
                    logger.LogWarning("Soft delete failed. Position with ID {PositionId} not found", id);
                    return Results.NotFound($"Position with ID {id} not found");
                }

                logger.LogInformation("Position with ID {PositionId} soft deleted successfully", id);
                return Results.NoContent();
            }).WithTags("Position").WithDescription("Delete position");

            return group;
        }
    }
}
