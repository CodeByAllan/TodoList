using TodoList.Application.Dtos;
using TodoList.Application.Interfaces;

namespace TodoList.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var auth = app.MapGroup("/auth");
        auth.MapPost("/login", async (IAuthService _service, LoginRequestDto request) =>
{
    try
    {
        string? token = await _service.LoginAsync(request);
        if (token != null)
        {
            return Results.Ok(new { token });
        }
        else
        {
            return Results.Unauthorized();
        }
    }
    catch (Exception ex)
    {
        return Results.InternalServerError(new { message = ex.Message });
    }
}).WithName("Login");

        auth.MapPost("/register", async (IAuthService _service, RegisterRequestDto request) =>
{
    try
    {
        string? token = await _service.RegisterAsync(request);
        if (string.IsNullOrEmpty(token)) return Results.BadRequest(new { message = "Error creating user." });

        return Results.Created($"/auth/register", new { token });
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { message = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { message = ex.Message });
    }
}).WithName("Register");
    }
}