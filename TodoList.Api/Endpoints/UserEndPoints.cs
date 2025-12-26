using TodoList.Application.Dtos;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var users = app.MapGroup("/users").RequireAuthorization("OwnerOnly").WithTags("Users");
        
        users.MapGet("/{id}", async (IUserService _service, int id) =>
        {
            try
            {
                User? user = await _service.GetByIdAsync(id);
                if (user != null)
                {
                    return Results.Ok(user);
                }
                else
                {
                    return Results.NotFound();
                }
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
        }).WithName("GetUserById")
        .WithSummary("Retrieves a user by their ID.")
        .WithDescription("This endpoint allows an authorized user to retrieve a user's details by providing their ID.")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces(StatusCodes.Status401Unauthorized);

        users.MapPut("/{id}", async (IUserService _service, int id, UpdateUserDto request) =>
        {
            try
            {
                User user = await _service.UpdateAsync(id, request);
                return Results.Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
        }).WithName("UpdateUser")
        .WithSummary("Updates a user's information.")
        .WithDescription("This endpoint allows an authorized user to update a user's information by providing their ID and the updated data.")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces(StatusCodes.Status401Unauthorized)
        .Accepts<UpdateUserDto>("application/json");

        users.MapDelete("/{id}", async (IUserService _service, int id) =>
        {
            try
            {
                await _service.DeleteAsync(id);
                return Results.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
        }).WithName("DeleteUser")
        .WithSummary("Deletes a user by their ID.")
        .WithDescription("This endpoint allows an authorized user to delete a user by providing their ID.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}