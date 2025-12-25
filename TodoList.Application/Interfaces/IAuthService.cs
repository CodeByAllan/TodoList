using TodoList.Application.Dtos;

namespace TodoList.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<string?> RegisterAsync(RegisterRequestDto registerRequestDto);
    }
}