using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using TodoItem.Infrastructure;
using TodoItems.Core;

namespace TodoItem.IntegrationTest;

public class TodoItemMongoRepositoryTest: IAsyncLifetime
{
    private readonly TodoItemMongoRepository _mongoRepository;
    private IMongoCollection<TodoItemPo> _mongoCollection;


    public TodoItemMongoRepositoryTest()
    {
        var mockSettings = new Mock<IOptions<TodoStoreDatabaseSettings>>();
        
        mockSettings.Setup(s => s.Value).Returns(new TodoStoreDatabaseSettings
        {
            ConnectionString = "mongodb://localhost:27017/",
            DatabaseName = "TodoTestStore",
            TodoItemsCollectionName = "Todos"
        });

        // 初始化 TodoService
        _mongoRepository = new TodoItemMongoRepository(mockSettings.Object);
        
        var mongoClient = new MongoClient("mongodb://localhost:27017/");
        var mongoDatabase = mongoClient.GetDatabase("TodoTestStore");
        _mongoCollection = mongoDatabase.GetCollection<TodoItemPo>("Todos");
    }
    
    // IAsyncLifetime 中的 InitializeAsync 方法在每个测试前运行
    public async Task InitializeAsync()
    {
        // 清空集合
        await _mongoCollection.DeleteManyAsync(FilterDefinition<TodoItemPo>.Empty);
    }

    // DisposeAsync 在测试完成后运行（如果有需要的话）
    public Task DisposeAsync() => Task.CompletedTask;


    [Fact]
    public async void should_return_item_by_id_1()
    {
        var todoItemPo = new TodoItemPo{
            Id = "5f9a7d8e2d3b4a1eb8a7d8e2", 
            Description = "Buy groceries",
            IsComplete = false
        };;
        await _mongoCollection.InsertOneAsync(todoItemPo);
        var todoItem = await _mongoRepository.FindById("5f9a7d8e2d3b4a1eb8a7d8e2");
        
        Assert.NotNull(todoItem);
        Assert.Equal("5f9a7d8e2d3b4a1eb8a7d8e2", todoItem.Id);
        Assert.Equal("Buy groceries", todoItem.Description);
    }

    [Fact]
    public async void should_return_item_number_on_the_duedate()
    {
        var dueDate = new DateTime(2024, 11, 1);
        var todoItemPo = new TodoItemPo
        {
            Id = "5f9a7d8e2d3b4a1eb8a7d8e2",
            Description = "Buy groceries",
            DueDate = new DateTime(2024, 11, 1)
        };
        await _mongoCollection.InsertOneAsync(todoItemPo);
        var count = await _mongoRepository.CountTodoItemsOnTheSameDueDate(dueDate);

        Assert.Equal(1, count);
    }

    [Fact]
    public async Task GetTodoItemsDueInNextFiveDays_ReturnsCorrectItems()
    {
        // Arrange
        var today = DateTime.UtcNow.Date;
        await _mongoCollection.InsertOneAsync(new TodoItemPo {
            Id = ObjectId.GenerateNewId().ToString(),
            DueDate = today.AddDays(4) 
        });
        await _mongoCollection.InsertOneAsync(new TodoItemPo
        {
            Id = ObjectId.GenerateNewId().ToString(),
            DueDate = today.AddDays(5)
        });
        await _mongoCollection.InsertOneAsync(new TodoItemPo
        {
            Id = ObjectId.GenerateNewId().ToString(),
            DueDate = today.AddDays(6)
        });

        // Act
        var result = await _mongoRepository.GetTodoItemsDueInNextFiveDays();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, item => item.DueDate.Date == today.AddDays(4).Date);
        Assert.Contains(result, item => item.DueDate.Date == today.AddDays(5).Date);
    }
}