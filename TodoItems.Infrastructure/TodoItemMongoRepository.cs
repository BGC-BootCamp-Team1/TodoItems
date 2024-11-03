using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoItems.Core.Model;
using TodoItems.Core.Repository;

namespace TodoItems.Infrastructure;

public class TodoItemMongoRepository : ITodoItemsRepository
{
    private readonly IMongoCollection<TodoItemPo> _todosCollection;

    public TodoItemMongoRepository(IOptions<TodoStoreDatabaseSettings> todoStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(todoStoreDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(todoStoreDatabaseSettings.Value.DatabaseName);
        _todosCollection = mongoDatabase.GetCollection<TodoItemPo>(todoStoreDatabaseSettings.Value.TodoItemsCollectionName);
    }

    public async Task<TodoItem> FindById(string? id)
    {
        FilterDefinition<TodoItemPo> filter = Builders<TodoItemPo>.Filter.Eq(x => x.Id, id);
        TodoItemPo todoItemPo = await _todosCollection.Find(filter).FirstOrDefaultAsync();

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
        FilterDefinition<TodoItemPo> filter = Builders<TodoItemPo>
            .Filter.And(
                Builders<TodoItemPo>.Filter.Eq(x => x.UserId, userId),
                Builders<TodoItemPo>.Filter.Eq(x => x.DueDay, dueDay.Date) // 确保仅比较日期部分
            );

        // 执行查询并转换为 TodoItem 类型的列表
        var todoItemsPoList = _todosCollection.Find(filter).ToList();
        var todoItems = todoItemsPoList.Select(TodoMapper.ToItem).ToList();
        return todoItems;

    }

    public TodoItem Save(TodoItem todoItem)
    {
        _todosCollection.InsertOne(TodoMapper.ToPo(todoItem));
        FilterDefinition<TodoItemPo> filter = Builders<TodoItemPo>.Filter.Eq(x => x.Id, todoItem.Id);
        TodoItemPo todoItemPo = _todosCollection.Find(filter).FirstOrDefault();

        return TodoMapper.ToItem(todoItemPo!);
    }

    public List<TodoItem> FindTodoItemsInFiveDaysByUserId(string userId)
    {
        DateTime startDate = DateTime.Today.AddDays(1);
        DateTime endDate = DateTime.Today.AddDays(5);

        FilterDefinition<TodoItemPo> filter = Builders<TodoItemPo>
            .Filter.And(
                Builders<TodoItemPo>.Filter.Eq(x => x.UserId, userId),
                Builders<TodoItemPo>.Filter.Gte(x => x.DueDay, startDate),
                Builders<TodoItemPo>.Filter.Lte(x => x.DueDay, endDate)
            );
        
        var todoItemsPoList = _todosCollection.Find(filter).ToList();
        var todoItems = todoItemsPoList.Select(TodoMapper.ToItem).ToList();

        return todoItems;

    }
}
