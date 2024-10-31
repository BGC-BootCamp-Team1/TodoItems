using TodoItems.Core;

namespace TodoItems.Test;

public class TodoItemTest
{
    [Fact]
    public void should_get_return_todo_item()
    {
        const string description = "this is description";
        const string type = "this is type";
        var todoItem = new TodoItem(description, type);
        Assert.Equal(type, todoItem.Type);
        Assert.Equal(description, todoItem.Description);
    }
}