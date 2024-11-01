using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using TodoItems.Infrastructure;

namespace TodoItems.IntegrationTest;

public class TodoItemMongoRepositoryTest : IAsyncLifetime
{
    private readonly TodoItemMongoRepository _mongoRepository;
    private IMongoCollection<TodoItemPo> _mongoCollection;


    public TodoItemMongoRepositoryTest()
    {
        var mockSettings = new Mock<IOptions<TodoStoreDatabaseSettings>>();

        mockSettings.Setup(s => s.Value).Returns(new TodoStoreDatabaseSettings
        {
            ConnectionString = "mongodb://sliu40:sliu40@47.116.197.93:27017",
            DatabaseName = "TodoTestStore",
            TodoItemsCollectionName = "Todos"
        });

        // 初始化 TodoService
        _mongoRepository = new TodoItemMongoRepository(mockSettings.Object);

        var mongoClient = new MongoClient("mongodb://sliu40:sliu40@47.116.197.93:27017");
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
        var todoItemPo = new TodoItemPo
        {
            Id = "5f9a7d8e2d3b4a1eb8a7d8e2",
            Description = "Buy groceries",
            userId = "user1",
        }; ;
        await _mongoCollection.InsertOneAsync(todoItemPo);
        var todoItem = await _mongoRepository.FindById("5f9a7d8e2d3b4a1eb8a7d8e2");

        Assert.NotNull(todoItem);
        Assert.Equal("5f9a7d8e2d3b4a1eb8a7d8e2", todoItem.Id);
        Assert.Equal("Buy groceries", todoItem.Description);
    }
}