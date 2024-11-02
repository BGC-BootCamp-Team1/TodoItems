using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoItems.Core;

namespace TodoItem.Infrastructure;

public class TodoItemMongoRepository : ITodoRepository
{
    private readonly IMongoCollection<TodoItemPo?> _todosCollection;

    public TodoItemMongoRepository(IOptions<TodoStoreDatabaseSettings> todoStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(todoStoreDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(todoStoreDatabaseSettings.Value.DatabaseName);
        _todosCollection = mongoDatabase.GetCollection<TodoItemPo>(todoStoreDatabaseSettings.Value.TodoItemsCollectionName);
    }

    public int CountTodoItemsOnDueDate(DateTime dueDate)
    {
        DateTime startOfDay = dueDate.Date; 
        DateTime endOfDay = dueDate.Date.AddDays(1).AddTicks(-1); 

        FilterDefinition<TodoItemPo?> filter = Builders<TodoItemPo>.Filter.And(
            Builders<TodoItemPo>.Filter.Gte(x => x.DueDate, startOfDay),
            Builders<TodoItemPo>.Filter.Lte(x => x.DueDate, endOfDay)
        );

        return (int)_todosCollection.CountDocuments(filter);
    }

    public async Task Save(TodoItemObject todoItem)
    {

        await _todosCollection.InsertOneAsync(ConvertToTodoItemPo(todoItem));
    }


    public async Task Replace(TodoItemObject todoItem)
    {
       
        var filter = Builders<TodoItemPo>.Filter.Eq(x => x.Id, todoItem.Id);

        
        var update = Builders<TodoItemPo>.Update
            .Set(x => x.Description, todoItem.Description)
            .Set(x => x.ModificationTimestamps, todoItem.ModificationTimestamps);

        
        await _todosCollection.UpdateOneAsync(filter, update);
    }



    public async Task<TodoItemObject> FindById(string? id)
    {
        FilterDefinition<TodoItemPo?> filter = Builders<TodoItemPo>.Filter.Eq(x => x.Id, id);
        TodoItemPo? todoItemPo = await _todosCollection.Find(filter).FirstOrDefaultAsync();

        
        TodoItemObject todoItem = ConvertToTodoItem(todoItemPo);
        return todoItem;
    }

    private TodoItemObject ConvertToTodoItem(TodoItemPo? todoItemPo)
    {
        if (todoItemPo == null) return null;

        return new TodoItemObject()
        {
            Id = todoItemPo.Id,
            Description = todoItemPo.Description,
            ModificationTimestamps = todoItemPo.ModificationTimestamps,
            DueDate = todoItemPo.DueDate
        };
    }

    private TodoItemPo ConvertToTodoItemPo(TodoItemObject todoItemObject)
    {
        return new TodoItemPo
        {
            Id = todoItemObject.Id,
            Description = todoItemObject.Description,
            ModificationTimestamps = todoItemObject.ModificationTimestamps,
            DueDate = (DateTime)todoItemObject.DueDate,
        };
    }
       

        


}
