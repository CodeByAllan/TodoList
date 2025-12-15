using TodoList.Application.Dtos;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var users = app.MapGroup("/users");
        users.MapPost("/", async (IUserService _service, CreateUserDto request) =>
        {
            try
            {
                User newUser = await _service.CreateAsync(request);
                return Results.Created($"/users/{newUser.ID}", newUser);
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
        }).WithName("CreateUser");

        users.MapGet("/", async (IUserService _service) =>
       {
           try { return Results.Ok(await _service.GetAllAsync()); }
           catch (Exception ex)
           {
               return Results.InternalServerError(new { message = ex.Message });
           }
       }).WithName("GetAllUser");
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
        }).WithName("GetUserById");
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
        }).WithName("UpdateUser");
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
        }).WithName("DeleteUser");
    }
}