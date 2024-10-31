using Moq;
using TodoItems.Core.BizException;
using TodoItems.Core.Model;
using TodoItems.Core.Repository;
using TodoItems.Core.Service;

namespace TodoItems.Test;
public class TodoItemServiceTest
{
    private TodoItemService _service;
    private readonly Mock<ITodosRepository> _mockedTodosRepository;

    public TodoItemServiceTest() {
        _mockedTodosRepository= new Mock<ITodosRepository>();
        _service = new TodoItemService(_mockedTodosRepository.Object);
    }

    [Fact]
    public void should_contains_todoItem_when_create()
    {
        _mockedTodosRepository.Setup(
            repository =>
                repository.FindAllTodoItemsByUserIdAndDueDay(
                    It.IsAny<string>(), It.IsAny<DateOnly>()
                )).Returns(
            [new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now), "user1")]);
        _mockedTodosRepository.Setup(
            repository =>
                repository.Insert(
                    It.IsAny<TodoItem>()
                )).Returns(true);

        _service = new TodoItemService(_mockedTodosRepository.Object);

        var todoItem = _service.Create("Des", DateOnly.FromDateTime(DateTime.Now), "user1");

        Assert.NotNull(todoItem);
    }

    [Fact]
    public void Can_create_items_10_dueDay()
    {
        _mockedTodosRepository.Setup(
            repository =>
                repository.FindAllTodoItemsByUserIdAndDueDay(
                    It.IsAny<string>(), It.IsAny<DateOnly>()
                )).Returns(
            [   new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1"),
                new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
                new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
                new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
                new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
                new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
            ]);
        _mockedTodosRepository.Setup(
            repository =>
                repository.Insert(
                    It.IsAny<TodoItem>()
                )).Returns(true);

        _service = new TodoItemService(_mockedTodosRepository.Object);

        var todoItem = _service.Create("Des", DateOnly.FromDateTime(DateTime.Now), "user1");

        Assert.NotNull(todoItem);
    }

    [Fact]
    public void Cannot_create_items_same_dueDay()
    {
        _mockedTodosRepository.Setup(
            repository =>
                repository.FindAllTodoItemsByUserIdAndDueDay(
                    It.IsAny<string>(), It.IsAny<DateOnly>()
                )).Returns(
        [   new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1"),
            new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
            new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
            new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
            new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
            new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
            new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
            new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
            new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
            new TodoItem("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user2"),
        ]);
        _mockedTodosRepository.Setup(
            repository =>
                repository.Insert(
                    It.IsAny<TodoItem>()
                )).Returns(true);

        _service = new TodoItemService(_mockedTodosRepository.Object);

        Assert.Throws<MaximumSameDueDayException>(() =>
        {
             _service.Create("Des", DateOnly.FromDateTime(DateTime.Now), "user1");
        });

        
    }

    [Fact]
    public void dueDay_canot_earlier_than_today()
    {
        Assert.Throws<DueDayEarlyException>(() =>
        {
            _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), "user1");
        });

    }




}

