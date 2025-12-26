using System.Security.Claims;
using TodoList.Application.Dtos;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using ToDoList.Utils;

namespace TodoList.Api.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this WebApplication app)
    {
        var todos = app.MapGroup("/me/todos").RequireAuthorization().WithTags("Todos");

        todos.MapPost("/", async (ITodoItemService _service, CreateTodoItemDto request, ClaimsPrincipal claimsPrincipal) =>
        {
            try
            {
                TodoItem newItem = await _service.CreateAsync(request, claimsPrincipal.GetUserId());
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
        })
        .WithName("CreateTodoItem")
        .WithSummary("Creates a new todo item for the authenticated user.")
        .WithDescription("This endpoint allows an authenticated user to create a new todo item by providing the necessary details in the request body.")
        .Produces<TodoItem>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces(StatusCodes.Status401Unauthorized)
        .Accepts<CreateTodoItemDto>("application/json");

        todos.MapGet("/", async (ITodoItemService _service, ClaimsPrincipal claimsPrincipal) =>
        {
            try { return Results.Ok(await _service.GetAllAsync(claimsPrincipal.GetUserId())); }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
        })
        .WithName("GetAllTodoItem")
        .WithSummary("Retrieves all todo items for the authenticated user.")
        .WithDescription("This endpoint allows an authenticated user to retrieve all their todo items.")
        .Produces<List<TodoItem>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces(StatusCodes.Status401Unauthorized);

        todos.MapGet("/{id}", async (ITodoItemService _service, int id, ClaimsPrincipal claimsPrincipal) =>
        {
            try
            {
                TodoItem? todoItem = await _service.GetByIdAsync(id, claimsPrincipal.GetUserId());
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
        })
        .WithName("GetTodoItemById")
        .WithSummary("Retrieves a specific todo item by ID for the authenticated user.")
        .WithDescription("This endpoint allows an authenticated user to retrieve a specific todo item by its ID.")
        .Produces<TodoItem>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces(StatusCodes.Status401Unauthorized);

        todos.MapPut("/{id}", async (ITodoItemService _service, int id, UpdateTodoItemDto request, ClaimsPrincipal claimsPrincipal) =>
        {
            try
            {
                TodoItem todoItem = await _service.UpdateAsync(id, request, claimsPrincipal.GetUserId());
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
        })
        .WithName("UpdateTodoItem")
        .WithSummary("Updates a specific todo item by ID for the authenticated user.")
        .WithDescription("This endpoint allows an authenticated user to update a specific todo item by its ID.")
        .Produces<TodoItem>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces(StatusCodes.Status401Unauthorized)
        .Accepts<UpdateTodoItemDto>("application/json");

        todos.MapDelete("/{id}", async (ITodoItemService _service, int id, ClaimsPrincipal claimsPrincipal) =>
        {
            try
            {
                await _service.DeleteAsync(id, claimsPrincipal.GetUserId());
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
        })
        .WithName("DeleteTodoItem")
        .WithSummary("Deletes a specific todo item by ID for the authenticated user.")
        .WithDescription("This endpoint allows an authenticated user to delete a specific todo item by its ID.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}