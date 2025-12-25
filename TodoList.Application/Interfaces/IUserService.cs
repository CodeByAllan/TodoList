using TodoList.Application.Dtos;
using TodoList.Domain.Entities;

namespace TodoList.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(int id);
    Task<User> UpdateAsync(int id, UpdateUserDto updateUserDto);
    Task DeleteAsync(int id);
}