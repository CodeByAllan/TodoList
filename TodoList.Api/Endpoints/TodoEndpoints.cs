using TodoList.Application.Dtos;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Api.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this WebApplication app)
    {
        var todos = app.MapGroup("/todos").RequireAuthorization();

        todos.MapPost("/", async (ITodoItemService _service, CreateTodoItemDto request) =>
        {
            try
            {
                TodoItem newItem = await _service.CreateAsync(request);
                return Results.Created($"/todos/{newItem.ID}", newItem);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
        }).WithName("CreateTodoItem");

        todos.MapGet("/", async (ITodoItemService _service) =>
        {
            try { return Results.Ok(await _service.GetAllAsync()); }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
        }).WithName("GetAllTodoItem");

        todos.MapGet("/{id}", async (ITodoItemService _service, int id) =>
        {
            try
            {
                TodoItem? todoItem = await _service.GetByIdAsync(id);
                if (todoItem != null)
                {
                    return Results.Ok(todoItem);
                }
                else
                {
                    return Results.NotFound();
                }
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
        });

        todos.MapPut("/{id}", async (ITodoItemService _service, int id, UpdateTodoItemDto request) =>
        {
            try
            {
                TodoItem todoItem = await _service.UpdateAsync(id, request);
                return Results.Ok(todoItem);
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
        });
        todos.MapDelete("/{id}", async (ITodoItemService _service, int id) =>
        {
            try
            {
                await _service.DeleteAsync(id);
                return Results.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
        });
    }
}