using TodoList.Application.Dtos;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;

namespace TodoList.Application.Services;

public class UserService(IUserRepository _repository) : IUserService
{
    public async Task<User> CreateAsync(CreateUserDto createUserDto)
    {
        User? user = await _repository.GetByUsernameAsync(createUserDto.Username);
        if (user != null)
        {
            throw new InvalidOperationException($"Username '{createUserDto.Username}' It is already in use.");
        }
        User newUser = new(firstName: createUserDto.FirstName, lastName: createUserDto.LastName, username: createUserDto.Username, password: createUserDto.Password);
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
        if (updateUserDto.Username != null && user.Username != updateUserDto.Username)
        {
            User? alreadyUser = await _repository.GetByUsernameAsync(updateUserDto.Username);
            if (alreadyUser != null && alreadyUser.ID != user.ID)
            {
                throw new InvalidOperationException($"Username '{updateUserDto.Username}' It is already in use by another user.");
            }
            user.UpdateUsername(updateUserDto.Username);
        }
        if (updateUserDto.FirstName != null)
        {
            user.UpdateFirstName(updateUserDto.FirstName);
        }
        if (updateUserDto.LastName != null)
        {
            user.UpdateLastName(updateUserDto.LastName);
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