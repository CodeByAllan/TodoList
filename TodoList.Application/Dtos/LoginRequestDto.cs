namespace TodoList.Application.Dtos;

public record LoginRequestDto
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}
