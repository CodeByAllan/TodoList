using TodoList.Application.Dtos;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;

namespace TodoList.Application.Services;

public class UserService(IUserRepository _repository) : IUserService
{
    public async Task<User> CreateAsync(CreateUserDto createUserDto)
    {
        User newUser = new(firstName: createUserDto.FirstName, lastName: createUserDto.LastName, username: createUserDto.Userame, password: createUserDto.Password);
        await _repository.AddAsync(newUser);
        await _repository.SaveChangesAsync();
        return newUser;
    }
    public async Task DeleteAsync(int id)
    {
        User user = await GetByIdAsync(id);
        await _repository.DeleteAsync(user);
        await _repository.SaveChangesAsync();
    }
    public Task<IEnumerable<User>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }
    public async Task<User> GetByIdAsync(int id)
    {
        User user = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"User with Id {id} not found!");
        return user;
    }
    public async Task<User> UpdateAsync(int id, UpdateUserDto updateUserDto)
    {
        User user = await GetByIdAsync(id);
        if (updateUserDto.FirstName != null)
        {
            user.UpdateFirstName(updateUserDto.FirstName);
        }
        if (updateUserDto.LastName != null)
        {
            user.UpdateLastName(updateUserDto.LastName);
        }
        if (updateUserDto.Userame != null)
        {
            user.UpdateUsername(updateUserDto.Userame);
        }
        if (updateUserDto.Password != null)
        {
            user.UpdatePassword(updateUserDto.Password);
        }
        await _repository.UpdateAsync(user);
        await _repository.SaveChangesAsync();
        return user;
    }
}