using Moq;
using TodoItems.Core.BizException;
using TodoItems.Core.Model;
using TodoItems.Core.Repository;
using TodoItems.Core.Service;

namespace TodoItems.Test;
public class TodoItemServiceTest
{
    private readonly TodoItemService _service;
    private readonly Mock<ITodosRepository> _mockedTodosRepository;

    public TodoItemServiceTest() {
        _mockedTodosRepository= new Mock<ITodosRepository>();
        _service = new TodoItemService(_mockedTodosRepository.Object);
    }

    [Fact]
    public void should_contains_todoItem_when_create()
    {
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now), "user1");
        Assert.Single(_service.itemList);
    }

    [Fact]
    public void Can_create_items_10_dueDay()
    {
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(2)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(3)), "user2");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(4)), "user3");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(5)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(6)), "user4");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(7)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(8)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(9)), "user1");

        Assert.Equal(10,_service.itemList.Count);
    }

    [Fact]
    public void Cannot_create_items_same_dueDay()
    {
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1");
        _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1");

        Assert.Throws<NotificationException>(() =>
        {
            _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "user1");
        });

    }

    [Fact]
    public void dueDay_canot_earlier_than_today()
    {
        Assert.Throws<NotificationException>(() =>
        {
            _service.Create("Des", DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), "user1");
        });

    }




}

