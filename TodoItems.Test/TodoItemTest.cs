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
        todoItem.EditItem(updateDesp, updateType);

        Assert.Equal(updateType, todoItem.Type);
        Assert.Equal(updateDesp, todoItem.Description);
    }

    [Fact]
    public void should_record_modification_frequency()
    {
        TodoItem todoItem = new TodoItem(_description, _type);
        string updateDesp = "new content";
        string updateType = "new type";

        todoItem.EditItem(updateDesp, updateType);
        todoItem.EditItem(updateDesp, updateType);
        todoItem.EditItem(updateDesp, updateType);

        Assert.Equal(3, todoItem.ModificationRecords.Count());
    }

    [Fact]
    public void should_limit_daily_modification_frequency_up_to_3()
    {
        TodoItem todoItem = new TodoItem(_description, _type);
        string updateDesp = "new content";
        string updateType = "new type";
        string errMsg;

        bool res = todoItem.TriggerModification(updateDesp, updateType, out errMsg);
        res = res ? todoItem.TriggerModification(updateDesp, updateType, out errMsg) : false;
        res = res ? todoItem.TriggerModification(updateDesp, updateType, out errMsg) : false;

        Assert.True(res);

        res = todoItem.TriggerModification(updateDesp,updateType, out errMsg);
        Assert.False(res);
        string expectedErrMsg = "You have reached the maximum number of modifications for today. Please try agian tomorrow.";
        Assert.Equal(expectedErrMsg, errMsg);
    }    

}