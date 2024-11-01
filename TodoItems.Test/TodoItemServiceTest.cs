using TodoItems.Core;
using Moq;
using Moq.Protected;
using Microsoft.VisualBasic;
using TodoItems.Core.ApplicationException;
using static TodoItems.Core.Constants;

namespace TodoItems.Test;

public class TodoItemServiceTest
{
    private readonly Mock<ITodosRepository> _mockRepository = new Mock<ITodosRepository>();
    private const string _description = "test description";
    private readonly DateTime _dueDate = DateTime.Today.AddDays(10);

    [Fact]
    public void Should_create_todo_item()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        var newItem = todoService.Create(_description, _dueDate, DueDateSetStrategy.Manual);

        Assert.Equal(_description, newItem.Description);
        Assert.Equal(_dueDate, newItem.DueDate);
        _mockRepository.Verify(repo => repo.Create(It.IsAny<TodoItem>()), Times.Once);
    }

    [Fact]
    public void Should_throw_exception_when_exceed_due_date_limit()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        var maxItemsPerDueDay = MAX_ITEMS_PER_DUE_DATE;
        _mockRepository.Setup(repo => repo.GetCountByDueDate(_dueDate)).Returns(maxItemsPerDueDay);
        var expectedErrMsg = $"Cannot create new Todo item completed on {_dueDate}, already reach max limit({maxItemsPerDueDay})";

        var exception = Assert.Throws<MaxItemsPerDueDateReachedException>(() => todoService.Create(_description, _dueDate, DueDateSetStrategy.Manual));
        Assert.Equal(expectedErrMsg, exception.Message);
    }

    [Fact]
    public void Should_not_create_item_when_due_date_earlier_than_creation_date()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        var earlyDueDate = DateTime.Today.AddDays(-5);
        var expectedErrMsg = "Cannot create todo item that due date earlier than creation date";
        
        var exception = Assert.Throws<DueDateEarlierThanCreationDateException>(() => todoService.Create(_description, earlyDueDate, DueDateSetStrategy.Manual));
        Assert.Equal(expectedErrMsg, exception.Message);
    }

    [Fact]
    public void Should_return_item_when_due_date_is_null()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        var newItem = todoService.Create(_description, null);

        Assert.Equal(_description, newItem.Description);
        Assert.Null(newItem.DueDate);
    }

    [Fact]
    public void Should_return_item_when_due_date_set_to_today()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        var newItem = todoService.Create(_description, DateTime.Today);

        Assert.Equal(_description, newItem.Description);
    }

    [Fact]
    public void Should_use_manual_strategy_when_manual_set_and_specify_strategy()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        var newItem = todoService.Create(_description, _dueDate, DueDateSetStrategy.FirstDateOfNextFiveDays);

        Assert.Equal(_dueDate, newItem.DueDate);
    }

    [Fact]
    public void Should_return_item_with_due_date_set_to_first_available_date()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        MockRepoGetCount(DateTime.Today, new List<int> { 10, 14, 4, 15, 7 });

        var newItem = todoService.Create(_description, null, DueDateSetStrategy.FirstDateOfNextFiveDays);

        Assert.Equal(DateTime.Today.AddDays(2), newItem.DueDate);
    }

    [Fact]
    public void Should_return_item_with_due_date_set_to_most_free_available_date()
    {
        var todoService = new TodoItemService(_mockRepository.Object);
        MockRepoGetCount(DateTime.Today, new List<int> { 10, 1, 4, 15, 7 });

        var newItem = todoService.Create(_description, null, DueDateSetStrategy.MostFreeDateOfNextFiveDays);

        Assert.Equal(DateTime.Today.AddDays(1), newItem.DueDate);
    }

    private void MockRepoGetCount(DateTime startDate, List<int> returnCounts)
    {
        for (int i = 0; i < returnCounts.Count; i++)
        {
            var date = startDate.AddDays(i);
            _mockRepository.Setup(repo => repo.GetCountByDueDate(date)).Returns(returnCounts[i]);
        }
    }

}
