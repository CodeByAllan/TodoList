using TodoList.Application.Dtos;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;

namespace TodoList.Application.Services;

public class AuthService(IUserRepository _userRepository, IPasswordHashService _passwordHashService, ITokenService _tokenService) : IAuthService
{
    public async Task<string?> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var userIsExist = await _userRepository.GetByUsernameAsync(loginRequestDto.Username);
        if (userIsExist == null || !_passwordHashService.VerifyHashedPassword(userIsExist.Password, loginRequestDto.Password))
        {
            return null;
        }
        return _tokenService.CreateToken(userIsExist);
    }
    public async Task<string?> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        User? user = await _userRepository.GetByUsernameAsync(registerRequestDto.Username);
        if (user != null)
        {
            throw new InvalidOperationException($"Username '{registerRequestDto.Username}' It is already in use.");
        }
        string hashedPassword = _passwordHashService.HashPassword(registerRequestDto.Password);
        User newUser = new(firstName: registerRequestDto.FirstName, lastName: registerRequestDto.LastName, username: registerRequestDto.Username, password: hashedPassword);
        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveChangesAsync();
        return _tokenService.CreateToken(newUser);
    }
}