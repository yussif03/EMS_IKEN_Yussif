using EMS.API.DTOs;
using EMS.API.Handlers.PositionHandlers;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints
{
    public static class PositionEndpoints
    {
        public static RouteGroupBuilder MapPositionEndpoints(this RouteGroupBuilder group)
        {
            // GET ALL POSITIONS
            group.MapGet("/", async ([FromServices]GetPositionsHandler handler) =>
            {
                var positions = await handler.HandleAsync();
                return Results.Ok(positions);
            })
            .WithTags("Position")
            .WithDescription("Get all positions");


            // GET POSITION BY ID
            group.MapGet("/{id:int}", async (int id, [FromServices]GetPositionByIdHandler handler) =>
            {
                var position = await handler.HandleAsync(id);

                return position is null
                    ? Results.NotFound($"Position with ID {id} not found")
                    : Results.Ok(position);
            })
            .WithTags("Position")
            .WithDescription("Get position by id");


            // CREATE POSITION
            group.MapPost("/", async ([FromBody]CreatePositionDto dto, [FromServices]CreatePositionHandler handler) =>
            {
                var position = await handler.HandleAsync(dto);

                return Results.Created(
                    $"/api/positions/{position.Id}",
                    position);
            })
            .WithTags("Position")
            .WithDescription("Create position");


            // UPDATE POSITION
            group.MapPut("/{id:int}", async (int id, [FromBody]UpdatePositionDto dto, [FromServices]UpdatePositionHandler handler) =>
            {
                var updated = await handler.HandleAsync(id, dto);

                return updated is null
                    ? Results.NotFound($"Position with ID {id} not found")
                    : Results.Ok(updated);
            })
            .WithTags("Position")
            .WithDescription("Update position");


            // DELETE POSITION (Soft Delete)
            group.MapDelete("/{id:int}", async (int id, [FromServices]DeletePositionHandler handler) =>
            {
                var deleted = await handler.HandleAsync(id);

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
