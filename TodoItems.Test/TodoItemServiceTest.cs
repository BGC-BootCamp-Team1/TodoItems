using TodoItems.Core;
using Moq;
using Moq.Protected;

namespace TodoItems.Test;

public class TodoItemServiceTest
{
    private readonly Mock<ITodosRepository> _mockRepository;
    private readonly TodoItemService _todoService;
    private const string _description = "test description";
    private readonly DateTime _dueDate = DateTime.Today.AddDays(5);

    public TodoItemServiceTest()
    {
        _mockRepository = new Mock<ITodosRepository>();
        _todoService = new TodoItemService(_mockRepository.Object);

        //var todoItem = new TodoItem { Id = "1", Description = "Test", CreatedDate = DateTime.Now, IsDone = false };
        //mockRepository.Setup(repo => repo.GetTodoItemById("1")).Returns(todoItem);
    }

    [Fact]
    public void should_create_todo_item()
    {
        var newItem = _todoService.Create(_description, _dueDate);

        Assert.Equal(_description, newItem.Description);
        Assert.Equal(_dueDate, newItem.DueDate);
    }
}
