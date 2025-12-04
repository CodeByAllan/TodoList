using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities;
using TodoList.Domain.Interfaces;
using TodoList.Infrastructure.Persistence;

namespace TodoList.Infrastructure.Repositories;

public class TodoItemRepository(ApplicationDbContext _applicationDbContext) : ITodoItemRepository
{
    public async Task AddAsync(TodoItem todoItem)
    {
        await _applicationDbContext.TodoItems.AddAsync(todoItem);
    }

    public Task DeleteAsync(TodoItem todoItem)
    {
        _applicationDbContext.TodoItems.Remove(todoItem);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        return await _applicationDbContext.TodoItems.ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        return await _applicationDbContext.TodoItems.FindAsync(id);
    }
    public Task UpdateAsync(TodoItem todoItem)
    {
        _applicationDbContext.TodoItems.Update(todoItem);
        return Task.CompletedTask;
    }
}