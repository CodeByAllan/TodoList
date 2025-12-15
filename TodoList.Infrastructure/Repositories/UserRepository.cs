using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;

namespace TodoList.Infrastructure.Repositories;

public class UserRepository(Persistence.ApplicationDbContext _applicationDbContext) : IUserRepository
{
    public async Task AddAsync(User user)
    {
        await _applicationDbContext.Users.AddAsync(user);
    }
    public Task DeleteAsync(User user)
    {
        _applicationDbContext.Users.Remove(user);
        return Task.CompletedTask;
    }
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _applicationDbContext.Users.ToListAsync();
    }
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _applicationDbContext.Users.FindAsync(id);
    }
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _applicationDbContext.SaveChangesAsync();
    }
    public Task UpdateAsync(User user)
    {
        _applicationDbContext.Users.Update(user);
        return Task.CompletedTask;
    }
}