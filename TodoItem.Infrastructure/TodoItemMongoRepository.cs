using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoItems.Core;

namespace TodoItem.Infrastructure;

public class TodoItemMongoRepository: ITodosRepository
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
        TodoItems.Core.TodoItem todoItem = todoItemPo.ConvertToTodoItem();
        return todoItem;
    }



    public void Save(TodoItems.Core.TodoItem todoItem)
    {
        throw new NotImplementedException();
    }

    public int CountTodoItemsByDueDate(DateOnly dueDate)
    {
        FilterDefinition<TodoItemPo> filter = Builders<TodoItemPo>.Filter.Eq(x => x.DueDate, dueDate);
        return (int)_todosCollection.CountDocuments(filter);
    }
}
