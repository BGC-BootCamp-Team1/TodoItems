using TodoItems.Core;

namespace TodoItems.Test;

public class TodoItemTest
{
    private readonly string _description = "this is description";

    [Fact]
    public void should_get_return_todo_item()
    {
        TodoItem todoItem = new TodoItem(_description);
        Assert.Equal(_description, todoItem.Description);
    }

    [Fact]
    public void should_update_content_when_edit()
    {
        TodoItem todoItem = new TodoItem(_description);
        string updateDesp = "new content";
        string errMsg;
        todoItem.ModifyItem(updateDesp);

        Assert.Equal(updateDesp, todoItem.Description);
    }

    [Fact]
    public void should_record_modification_frequency()
    {
        TodoItem todoItem = new TodoItem(_description);
        string updateDesp = "new content";

        todoItem.ModifyItem(updateDesp);
        todoItem.ModifyItem(updateDesp);
        todoItem.ModifyItem(updateDesp);

        Assert.Equal(3, todoItem.ModificationRecords.Count());
    }

    [Fact]
    public void should_limit_daily_modification_frequency_up_to_3()
    {
        TodoItem todoItem = new TodoItem(_description);
        string updateDesp = "new content";
        string expectedErrMsg = "You have reached the maximum number of modifications for today. Please try agian tomorrow.";

        todoItem.ModifyItem(updateDesp);
        todoItem.ModifyItem(updateDesp);
        todoItem.ModifyItem(updateDesp);

        var exception = Assert.Throws<MaxModificationsReachedException>(() => todoItem.ModifyItem(updateDesp));
        Assert.Equal(expectedErrMsg, exception.Message);
    }
}