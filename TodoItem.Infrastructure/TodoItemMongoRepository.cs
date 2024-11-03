﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoItems.Core;

namespace TodoItem.Infrastructure;

public class TodoItemMongoRepository: ITodoItemsRepository
{
    private readonly IMongoCollection<TodoItemPo?> _todosCollection;
    
    public TodoItemMongoRepository(IOptions<TodoStoreDatabaseSettings> todoStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(todoStoreDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(todoStoreDatabaseSettings.Value.DatabaseName);
        _todosCollection = mongoDatabase.GetCollection<TodoItemPo>(todoStoreDatabaseSettings.Value.TodoItemsCollectionName);
    }

    public async Task<TodoItems.Core.TodoItem> FindById(string? id)
    {
        FilterDefinition<TodoItemPo?> filter = Builders<TodoItemPo>.Filter.Eq(x => x.Id, id);
        TodoItemPo? todoItemPo = await _todosCollection.Find(filter).FirstOrDefaultAsync();

        // 将 TodoItemPo 转换为 TodoItem
        TodoItems.Core.TodoItem todoItem = ConvertToTodoItem(todoItemPo);
        return todoItem;
    }

    private TodoItems.Core.TodoItem ConvertToTodoItem(TodoItemPo? todoItemPo)
    {
        if (todoItemPo == null) return null;

        return new TodoItems.Core.TodoItem
        {
            Id = todoItemPo.Id,
            Description = todoItemPo.Description,
            DueDate = todoItemPo.DueDate,
            CreatedTime = todoItemPo.CreatedTime

        };
    }

    public void Save(TodoItems.Core.TodoItem todoItem)
    {
        throw new NotImplementedException();
    }

    public async Task<long> CountTodoItemsOnTheSameDueDate(DateTime dueDate)
    {
        FilterDefinition<TodoItemPo?> filter = Builders<TodoItemPo>.Filter.Eq(x => x.DueDate, dueDate);
        var count = await _todosCollection.CountDocumentsAsync(filter);

        return count;
    }

    public async Task<List<TodoItems.Core.TodoItem>> GetTodoItemsDueInNextFiveDays()
    {
        var today = DateTime.Today.Date.ToUniversalTime();

        var filter = Builders<TodoItemPo>.Filter.And(
            Builders<TodoItemPo>.Filter.Gte(item => item.DueDate, today),
            Builders<TodoItemPo>.Filter.Lte(item => item.DueDate, today.AddDays(5)));

        var todoItemPos = await _todosCollection.Find(filter).ToListAsync();
        var todoItems = todoItemPos.Select(ConvertToTodoItem).ToList();
        return todoItems;
    }
}
