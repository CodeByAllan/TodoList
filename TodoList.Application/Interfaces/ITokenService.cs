using TodoList.Domain.Entities;

namespace TodoList.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
