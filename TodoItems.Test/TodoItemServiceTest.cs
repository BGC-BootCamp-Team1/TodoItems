using TodoItems.Core;
using Moq;
using Moq.Protected;
using Microsoft.VisualBasic;
using TodoItems.Core.ApplicationException;

namespace TodoItems.Test;

public class TodoItemServiceTest
{
    private readonly Mock<ITodosRepository> _mockRepository = new Mock<ITodosRepository>();
    private const string _description = "test description";
    private readonly DateTime _dueDate = DateTime.Today.AddDays(5);

    [Fact]
    public void Should_create_todo_item()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        var newItem = todoService.Create(_description, _dueDate);

        Assert.Equal(_description, newItem.Description);
        Assert.Equal(_dueDate, newItem.DueDate);
        _mockRepository.Verify(repo => repo.Create(It.IsAny<TodoItem>()), Times.Once);
    }

    [Fact]
    public void Should_throw_exception_when_exceed_due_date_limit()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        var maxItemsPerDueDay = todoService.MaxItemsPerDueDate;
        _mockRepository.Setup(repo => repo.GetCountByDueDate(_dueDate)).Returns(maxItemsPerDueDay);
        var expectedErrMsg = $"Cannot create new Todo item completed on {_dueDate}, already reach max limit({maxItemsPerDueDay})";

        var exception = Assert.Throws<MaxItemsPerDueDateReachedException>(() => todoService.Create(_description, _dueDate));
        Assert.Equal(expectedErrMsg, exception.Message);
    }

    [Fact]
    public void Should_not_create_item_when_due_date_earlier_than_creation_date()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        var earlyDueDate = DateTime.Today.AddDays(-5);
        var expectedErrMsg = "Cannot create todo item that due date earlier than creation date";
        
        var exception = Assert.Throws<DueDateEarlierThanCreationDateException>(() => todoService.Create(_description, earlyDueDate));
        Assert.Equal(expectedErrMsg, exception.Message);
    }
}
