using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using TodoItems.Core;

namespace TodoItem.Infrastructure;

public class TodoItemMongoRepository : ITodosRepository
{
    private readonly IMongoCollection<TodoItemPo?> _todosCollection;

    public TodoItemMongoRepository(IOptions<TodoStoreDatabaseSettings> todoStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(todoStoreDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(todoStoreDatabaseSettings.Value.DatabaseName);
        _todosCollection = mongoDatabase.GetCollection<TodoItemPo>(todoStoreDatabaseSettings.Value.TodoItemsCollectionName);
    }

    public async Task<TodoItems.Core.TodoItem> FindByIdAsync(string? id)
    {
        FilterDefinition<TodoItemPo?> filter = Builders<TodoItemPo>.Filter.Eq(x => x.Id, id);
        TodoItemPo? todoItemPo = await _todosCollection.Find(filter).FirstOrDefaultAsync();

        // 将 TodoItemPo 转换为 TodoItem
        TodoItems.Core.TodoItem? todoItem = todoItemPo?.ConvertToTodoItem();
        return todoItem;
    }



    public async Task SaveAsync(TodoItems.Core.TodoItem todoItem)
    {
        if (todoItem == null) return;

        var todoItemPo = new TodoItemPo
        {
            Id = todoItem.Id,
            Description = todoItem.Description,
            IsComplete = todoItem.IsComplete,
            DueDate = todoItem.DueDate,
            CreateTime = todoItem.CreateTime,
        };
        FilterDefinition<TodoItemPo?> filter = Builders<TodoItemPo>.Filter.Eq(x => x.Id, todoItem.Id);
        bool isNewTodo = await _todosCollection.Find(filter).FirstOrDefaultAsync() == null ? true : false;
        if (isNewTodo)
        {
            await _todosCollection.InsertOneAsync(todoItemPo);
        }
        else
        {
            var update = Builders<TodoItemPo>.Update
                .Set(x => x.Description, todoItemPo.Description)
                .Set(x => x.IsComplete, todoItemPo.IsComplete)
                .Set(x => x.DueDate, todoItemPo.DueDate)
                .Set(x => x.CreateTime, todoItemPo.CreateTime);
            await _todosCollection.UpdateOneAsync(filter, update);
        }
    }

    public int CountTodoItemsByDueDate(DateOnly dueDate)
    {
        FilterDefinition<TodoItemPo> filter = Builders<TodoItemPo>.Filter.Eq(x => x.DueDate, dueDate);
        return (int)_todosCollection.CountDocuments(filter);
    }
}
