using System.Security.Claims;
using TodoList.Application.Dtos;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using ToDoList.Utils;

namespace TodoList.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var users = app.MapGroup("/me").WithTags("Profile").RequireAuthorization();

        users.MapGet("/", async (IUserService _service, ClaimsPrincipal claimsPrincipal) =>
        {
            try
            {
                User user = await _service.GetByIdAsync(claimsPrincipal.GetUserId());
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
        }).WithName("GetMe")
        .WithSummary("Retrieves the current user's profile.")
        .WithDescription("This endpoint allows an authorized user to retrieve their own profile details.")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces(StatusCodes.Status401Unauthorized);

        users.MapPut("/", async (IUserService _service, ClaimsPrincipal claimsPrincipal, UpdateUserDto request) =>
        {
            try
            {
                User user = await _service.UpdateAsync(claimsPrincipal.GetUserId(), request);
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
        }).WithName("UpdateMe")
        .WithSummary("Updates the current user's information.")
        .WithDescription("This endpoint allows an authorized user to update their own information.")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces(StatusCodes.Status401Unauthorized)
        .Accepts<UpdateUserDto>("application/json");

        users.MapDelete("/", async (IUserService _service, ClaimsPrincipal claimsPrincipal) =>
        {
            try
            {
                await _service.DeleteAsync(claimsPrincipal.GetUserId());
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
        }).WithName("DeleteMe")
        .WithSummary("Deletes the current user.")
        .WithDescription("This endpoint allows an authorized user to delete their own account.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}