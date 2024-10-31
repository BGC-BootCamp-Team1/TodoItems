using TodoItems.Core;

namespace TodoItems.Test;

public class TodoItemTest
{
    private readonly string _description = "this is description";
    private readonly string _type = "this is type";

    [Fact]
    public void should_get_return_todo_item()
    {
        TodoItem todoItem = new TodoItem(_description, _type); 
        Assert.Equal(_type, todoItem.Type);
        Assert.Equal(_description, todoItem.Description);
    }

    [Fact]
    public void should_update_content_when_edit()
    {
        TodoItem todoItem = new TodoItem(_description, _type);
        string updateDesp = "new content";
        string updateType = "new type";
        todoItem.ModifyItem(updateDesp, updateType);

        Assert.Equal(updateType, todoItem.Type);
        Assert.Equal(updateDesp, todoItem.Description);
    }

    [Fact]
    public void should_record_modification_frequency()
    {
        TodoItem todoItem = new TodoItem(_description, _type);
        string updateDesp = "new content";
        string updateType = "new type";

        todoItem.ModifyItem(updateDesp, updateType);
        todoItem.ModifyItem(updateDesp, updateType);
        todoItem.ModifyItem(updateDesp, updateType);

        Assert.Equal(3, todoItem.ModificationRecords.Count());
    }
    
}