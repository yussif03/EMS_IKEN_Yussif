using EMS.API.Models;
using EMS.API.Models.DTOs;
using EMS.API.Repositories.Base;

namespace EMS.API.Endpoints
{
    public static class PositionEndpoints
    {
        public static RouteGroupBuilder MapPositionEndpoints(this RouteGroupBuilder group)
        {
            // GET ALL POSITIONS
            group.MapGet("/", async (IRepository<Position> repo) =>
            {
                var positions = await repo.GetAllAsync();
                return Results.Ok(positions);
            });

            // GET POSITION BY ID
            group.MapGet("/{id:int}", async (int id, IRepository<Position> repo) =>
            {
                var position = await repo.GetByIdAsync(id);
                return position is null
                    ? Results.NotFound($"Position with ID {id} not found")
                    : Results.Ok(position);
            });

            // CREATE POSITION
            group.MapPost("/", async (CreatePositionDto dto, IRepository<Position> repo) =>
            {
                var position = new Position
                {
                    Title = dto.Title,
                    MinSalary = dto.MinSalary,
                    MaxSalary = dto.MaxSalary,
                    IsDeleted = false
                };

                var newId = await repo.AddAsync(position);
                position.Id = newId;

                return Results.Created($"/api/positions/{newId}", position);
            });

            // UPDATE POSITION
            group.MapPut("/{id:int}", async (int id, UpdatePositionDto dto, IRepository<Position> repo) =>
            {
                var existingPosition = await repo.GetByIdAsync(id);
                if (existingPosition is null)
                {
                    return Results.NotFound($"Position with ID {id} not found");
                }

                existingPosition.Title = dto.Title;
                existingPosition.MinSalary = dto.MinSalary;
                existingPosition.MaxSalary = dto.MaxSalary;

                var updated = await repo.UpdateAsync(existingPosition);
                return updated
                    ? Results.Ok(existingPosition)
                    : Results.NotFound($"Position with ID {id} not found");
            });

            // DELETE POSITION (Soft Delete)
            group.MapDelete("/{id:int}", async (int id, IRepository<Position> repo) =>
            {
                var deleted = await repo.SoftDeleteAsync(id);
                return deleted
                    ? Results.NoContent()
                    : Results.NotFound($"Position with ID {id} not found");
            });

            return group;
        }
    }
}
