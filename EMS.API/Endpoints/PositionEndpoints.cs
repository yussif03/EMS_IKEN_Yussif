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
            })
            .WithTags("Position")
            .WithDescription("Get all positions");

            // GET POSITION BY ID
            group.MapGet("/{id:int}", async (int id, IRepository<Position> repo) =>
            {
                var position = await repo.GetByIdAsync(id);

                return position is null
                    ? Results.NotFound($"Position with ID {id} not found")
                    : Results.Ok(position);
            })
            .WithTags("Position")
            .WithDescription("Get position by id");

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

                position.Id = await repo.AddAsync(position);

                return Results.Created($"/api/positions/{position.Id}", position);
            })
            .WithTags("Position")
            .WithDescription("Create position");

            // UPDATE POSITION
            group.MapPut("/{id:int}", async (int id, UpdatePositionDto dto, IRepository<Position> repo) =>
            {
                var position = await repo.GetByIdAsync(id);
                if (position is null)
                    return Results.NotFound($"Position with ID {id} not found");

                position.Title = dto.Title;
                position.MinSalary = dto.MinSalary;
                position.MaxSalary = dto.MaxSalary;

                await repo.UpdateAsync(position);

                return Results.Ok(position);
            })
            .WithTags("Position")
            .WithDescription("Update position");

            // DELETE POSITION (Soft Delete)
            group.MapDelete("/{id:int}", async (int id, IRepository<Position> repo) =>
            {
                var deleted = await repo.SoftDeleteAsync(id);

                return deleted
                    ? Results.NoContent()
                    : Results.NotFound($"Position with ID {id} not found");
            })
            .WithTags("Position")
            .WithDescription("Delete position");

            return group;
        }
    }
}
