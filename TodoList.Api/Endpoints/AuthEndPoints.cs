using TodoList.Application.Dtos;
using TodoList.Application.Interfaces;

namespace TodoList.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var auth = app.MapGroup("/auth").WithTags("Authentication");
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
    })
    .WithName("Login")
    .WithSummary("Authenticates a user and returns a JWT token.")
    .WithDescription("This endpoint allows a user to log in by providing their credentials and returns a JWT token upon successful authentication.")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status500InternalServerError)
    .Accepts<LoginRequestDto>("application/json");

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
    })
    .WithName("Register")
    .WithSummary("Registers a new user and returns a JWT token.")
    .WithDescription("This endpoint allows a new user to register by providing the necessary details and returns a JWT token upon successful registration.")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status409Conflict)
    .Produces(StatusCodes.Status500InternalServerError)
    .Accepts<RegisterRequestDto>("application/json");
    }
}