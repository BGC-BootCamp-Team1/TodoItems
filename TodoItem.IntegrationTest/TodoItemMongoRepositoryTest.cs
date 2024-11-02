﻿using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using Moq;
using TodoItem.Infrastructure;
using TodoItems.Core;

namespace TodoItem.IntegrationTest;

public class TodoItemMongoRepositoryTest : IAsyncLifetime
{
    private readonly TodoItemMongoRepository _mongoRepository;
    private IMongoCollection<TodoItemPo> _mongoCollection;


    public TodoItemMongoRepositoryTest()
    {
        var mockSettings = new Mock<IOptions<TodoStoreDatabaseSettings>>();

        mockSettings.Setup(s => s.Value).Returns(new TodoStoreDatabaseSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "TodoTestStore",
            TodoItemsCollectionName = "Todos"
        });

        // 初始化 TodoService
        _mongoRepository = new TodoItemMongoRepository(mockSettings.Object);

        var mongoClient = new MongoClient("mongodb://localhost:27017");
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
        var ModificationsOnDifferentDay = new List<Modification>
        {
            new Modification(DateTime.Now.AddDays(-1).AddSeconds(-30)),
            new Modification(DateTime.Now.AddDays(-1)),
            new Modification(DateTime.Now),
            new Modification(DateTime.Now.AddSeconds(1))
        };

        var todoItemPo = new TodoItemPo
        {
            Id = "5f9a7d8e2d3b4a1eb8a7d8e2",
            Description = "Buy groceries",
            ModificationTimestamps = ModificationsOnDifferentDay,
            DueDate = DateTime.Now.AddDays(1)
        };
        await _mongoCollection.InsertOneAsync(todoItemPo);
        var todoItem = await _mongoRepository.FindById("5f9a7d8e2d3b4a1eb8a7d8e2");

        Assert.NotNull(todoItem);
        Assert.Equal("5f9a7d8e2d3b4a1eb8a7d8e2", todoItem.Id);
        Assert.Equal("Buy groceries", todoItem.Description);
    }

    [Fact]
    public async Task Should_Update_Item_Description_And_ModificationTimestamps()
    {
        // 准备测试数据
        var initialModifications = new List<Modification>
            {
                new Modification(DateTime.Now.AddDays(-1)),
                new Modification(DateTime.Now)
            };

        var todoItemPo = new TodoItemPo
        {
            Id = "5f9a7d8e2d3b4a1eb8a7d8e2",
            Description = "Initial Description",
            ModificationTimestamps = initialModifications,
            DueDate = DateTime.Now.AddDays(1)
        };

        await _mongoCollection.InsertOneAsync(todoItemPo);

        // 更新数据
        var updatedModifications = new List<Modification>
            {
                new Modification(DateTime.Now.AddDays(-1)),
                new Modification(DateTime.Now),
                new Modification(DateTime.Now.AddMinutes(1))
            };

        var updatedTodoItem = new TodoItemObject
        {
            Id = "5f9a7d8e2d3b4a1eb8a7d8e2",
            Description = "Updated Description",
            ModificationTimestamps = updatedModifications,
            DueDate = DateTime.Now.AddDays(1)
        };

        await _mongoRepository.Replace(updatedTodoItem);

        // 验证更新结果
        var filter = Builders<TodoItemPo>.Filter.Eq(x => x.Id, "5f9a7d8e2d3b4a1eb8a7d8e2");
        var result = await _mongoCollection.Find(filter).FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.Equal("Updated Description", result.Description);
        Assert.Equal(3, result.ModificationTimestamps.Count);
    }

}