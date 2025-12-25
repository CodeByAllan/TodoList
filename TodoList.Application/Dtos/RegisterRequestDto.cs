namespace TodoList.Application.Dtos;

public record RegisterRequestDto
{
    public string FirstName { get; init; } = string.Empty;
    public string? LastName { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}