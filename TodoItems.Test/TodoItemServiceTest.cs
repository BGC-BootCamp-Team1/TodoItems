﻿using Moq;
using TodoItems.Core;
using TodoItems.Core.Model;
using TodoItems.Core.Repository;
using TodoItems.Core.Service;

namespace TodoItems.Test;
public class TodoItemServiceTest
{
    private TodoItemService _service;
    private Mock<ITodoItemsRepository> _mockedTodosRepository;

    public TodoItemServiceTest() {
        _mockedTodosRepository= new Mock<ITodoItemsRepository>();

        _service = new TodoItemService(_mockedTodosRepository.Object);
    }

    [Fact]
    public void should_contains_todoItem_when_create()
    {
        _mockedTodosRepository.Setup(
            repository =>
                repository.FindAllTodoItemsByUserIdAndDueDay(
                    It.IsAny<string>(), It.IsAny<DateTime>()
                )).Returns(
            [new TodoItem("Des", DateTime.Today, "user1")]);
        _mockedTodosRepository.Setup(
            repository =>
                repository.Save(
                    It.IsAny<TodoItem>()
                )).Returns(new TodoItem("Des", DateTime.Today.AddDays(1), "user2"));


        var todoItem = _service.Create(OptionEnum.Manual,"Des", DateTime.Today, "user1");

        Assert.NotNull(todoItem);
    }

    [Fact]
    public void Can_create_items_with_dueDay()
    {
        _mockedTodosRepository.Setup(
            repository =>
                repository.Save(
                    It.IsAny<TodoItem>()
                )).Returns(new TodoItem("Des", DateTime.Today.AddDays(1), "user1"));

        _mockedTodosRepository.Setup(
            repository =>
                repository.FindAllTodoItemsByUserIdAndDueDay(
                    It.IsAny<string>(), It.IsAny<DateTime>()
                )).Returns(
            [   new TodoItem("Des", DateTime.Today.AddDays(1), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
                new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
                new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
                new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
                new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
            ]);

        var todoItem = _service.Create(OptionEnum.Manual, "Des", DateTime.Today.AddDays(1), "user1");

        Assert.NotNull(todoItem);
    }

    [Fact]
    public void Cannot_create_items_same_dueDay()
    {
        _mockedTodosRepository.Setup(
            repository =>
                repository.FindAllTodoItemsByUserIdAndDueDay(
                    It.IsAny<string>(), It.IsAny<DateTime>()
                )).Returns(
        [   new TodoItem("Des", DateTime.Today.AddDays(1), "user1"),
            new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
            new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
            new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
            new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
            new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
            new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
            new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
            new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
            new TodoItem("Des", DateTime.Today.AddDays(1), "user2"),
        ]);
        _mockedTodosRepository.Setup(
            repository =>
                repository.Save(
                    It.IsAny<TodoItem>()
                )).Returns(new TodoItem("Des", DateTime.Today.AddDays(1), "user2"));

        _service = new TodoItemService(_mockedTodosRepository.Object);

        Assert.Throws<MaximumSameDueDayException>(() =>
        {
             _service.Create(OptionEnum.Manual, "Des", DateTime.Today, "user1");
        });

        
    }

    [Fact]
    public void dueDay_canot_earlier_than_today()
    {
        Assert.Throws<DueDayEarlyException>(() =>
        {
            _service.Create(OptionEnum.Manual, "Des", DateTime.Today.AddDays(-1), "user1");
        });

    }

    [Fact]
    public void CreateTodoItem_GivenOptionANoDueDay_ShouldSuccess()
    {
        _mockedTodosRepository = new Mock<ITodoItemsRepository>();
        _mockedTodosRepository.Setup(
            repository =>
            repository.Save(
                It.IsAny<TodoItem>()
            ))
            .Returns(new TodoItem("Des", DateTime.Today.AddDays(1), "user1"));
        _mockedTodosRepository.Setup(
            repository =>
            repository.FindTodoItemsInFiveDaysByUserId(
                It.IsAny<string>()
            )
        ).Returns(
            [   new TodoItem("Des", DateTime.Today.AddDays(1), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(4), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(4), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(2), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(2), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(1), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(2), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(3), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(5), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(1), "user1"),
            ]
        );
        _service = new TodoItemService(_mockedTodosRepository.Object);

        TodoItem todoItem = _service.Create(OptionEnum.Latest, "Des",null,"user1");

        Assert.NotNull(todoItem);
        Assert.Equal(DateTime.Today.AddDays(1),todoItem.DueDay);

    }
    [Fact]
    public void CreateTodoItem_GivenOptionANoDueDay_ThrowError()
    {
        List<TodoItem> mockResList = new List<TodoItem>();

        for (int i = 1; i <= 5; i++)
        {
            for (int j = 0; j < Constants.MAX_DAY_SAME_DUEDAY; j++)
            {
                mockResList.Add(new TodoItem("Des", DateTime.Today.AddDays(i), "user1"));
            }
        }
        _mockedTodosRepository = new Mock<ITodoItemsRepository>();
        _mockedTodosRepository.Setup(
            repository =>
            repository.Save(
                It.IsAny<TodoItem>()
            ))
            .Returns(new TodoItem("Des", DateTime.Today.AddDays(1), "user2"));
        _mockedTodosRepository.Setup(
            repository =>
            repository.FindTodoItemsInFiveDaysByUserId(
                It.IsAny<string>()
            )
        ).Returns(mockResList);
        _service = new TodoItemService(_mockedTodosRepository.Object);


        Assert.Throws<MaximumSameDueDayException>(() =>
        {
            _service.Create(OptionEnum.Latest, "Des", null, "user1");
        });
    }
    [Fact]
    public void CreateTodoItem_GivenOptionBNoDueDay_ShouldSuccess()
    {
        _mockedTodosRepository = new Mock<ITodoItemsRepository>();
        _mockedTodosRepository.Setup(
            repository =>
            repository.Save(
                It.IsAny<TodoItem>()
            ))
            .Returns(new TodoItem("Des", DateTime.Today.AddDays(3), "user2"));
        _mockedTodosRepository.Setup(
            repository =>
            repository.FindTodoItemsInFiveDaysByUserId(
                It.IsAny<string>()
            )
        ).Returns(
            [   new TodoItem("Des", DateTime.Today.AddDays(1), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(4), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(4), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(2), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(2), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(1), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(2), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(3), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(5), "user1"),
                new TodoItem("Des", DateTime.Today.AddDays(1), "user1"),
            ]
        );
        _service = new TodoItemService(_mockedTodosRepository.Object);

        TodoItem todoItem = _service.Create(OptionEnum.Freest, "Des", null, "user1");

        Assert.Equal(DateTime.Today.AddDays(3), todoItem.DueDay);

    }
    [Fact]
    public void CreateTodoItem_GivenOptionBNoDueDay_ThrowError()
    {
        List<TodoItem> mockResList = new List<TodoItem>();

        for (int i = 1; i <= 5; i++)
        {
            for (int j = 0; j < Constants.MAX_DAY_SAME_DUEDAY; j++)
            {
                mockResList.Add(new TodoItem("Des", DateTime.Today.AddDays(i), "user1"));
            }
        }
        _mockedTodosRepository = new Mock<ITodoItemsRepository>();
        _mockedTodosRepository.Setup(
            repository =>
            repository.Save(
                It.IsAny<TodoItem>()
            ))
            .Returns(new TodoItem("Des", DateTime.Today.AddDays(1), "user2"));
        _mockedTodosRepository.Setup(
            repository =>
            repository.FindTodoItemsInFiveDaysByUserId(
                It.IsAny<string>()
            )
        ).Returns(mockResList);
        _service = new TodoItemService(_mockedTodosRepository.Object);


        Assert.Throws<MaximumSameDueDayException>(() =>
        {
            _service.Create(OptionEnum.Freest, "Des", null, "user1");
        });
    }






}

