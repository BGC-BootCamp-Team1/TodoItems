using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoItems.Core.Model;
using TodoItems.Core.Repository;

namespace TodoItems.Infrastructure;

public class TodoItemMongoRepository : ITodoItemsRepository
{
    private readonly IMongoCollection<TodoItemPo?> _todosCollection;

    public TodoItemMongoRepository(IOptions<TodoStoreDatabaseSettings> todoStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(todoStoreDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(todoStoreDatabaseSettings.Value.DatabaseName);
        _todosCollection = mongoDatabase.GetCollection<TodoItemPo>(todoStoreDatabaseSettings.Value.TodoItemsCollectionName);
    }

    public async Task<TodoItem> FindById(string? id)
    {
        FilterDefinition<TodoItemPo?> filter = Builders<TodoItemPo>.Filter.Eq(x => x.Id, id);
        TodoItemPo? todoItemPo = await _todosCollection.Find(filter).FirstOrDefaultAsync();

        // 将 TodoItemPo 转换为 TodoItem
        TodoItem todoItem = ConvertToTodoItem(todoItemPo);
        return todoItem;
    }

    private TodoItem ConvertToTodoItem(TodoItemPo? todoItemPo)
    {
        if (todoItemPo == null) return null;

        return new TodoItem (todoItemPo.Id,todoItemPo.Description,todoItemPo.DueDay,"user1");

    }
    
    public List<TodoItem> FindAllTodoItemsByUserIdAndDueDay(string userId, DateTime dueDay)
    {
        throw new NotImplementedException();
    }

    public TodoItem Save(TodoItem todoItem)
    {
        throw new NotImplementedException();
    }

    public List<TodoItem> FindTodoItemsInFiveDaysByUserId(string userId)
    {
        throw new NotImplementedException();
    }
}
