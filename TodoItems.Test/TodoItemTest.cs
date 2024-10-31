using TodoItems.Core;

namespace TodoItems.Test;

public class TodoItemTest
{
    [Fact]
    public void should_return_todoItem_when_create()
    {
        TodoItem item = new TodoItem();
        Assert.NotNull(item._id);
        Assert.NotNull(item.Description);
        Assert.NotNull(item.ModificationList);
    }
    [Fact]
    public void should_contain_1_modification_when_modify()
    {
        TodoItem item = new TodoItem();
        item.Modify("newDes");
        Assert.Single(item.ModificationList);
    }

    [Fact]
    public void should_throw_exception_when_more_than_3_today()
    {
        TodoItem item = new TodoItem();
        item.Modify("1");
        item.Modify("2");
        item.Modify("3");

        Assert.Throws<NotificationException>(() =>
        {
            item.Modify("4");
        });

    }

    [Fact]
    public void should_not_throw_exception_when_less_than_3_today()
    {
        TodoItem item = new TodoItem();
        item.Modify("1");
        item.Modify("2");
        item.Modify("3");

        item.ModificationList[0].TimeStamp = item.ModificationList[0].TimeStamp.AddDays(-1);
        item.ModificationList[1].TimeStamp = item.ModificationList[1].TimeStamp.AddDays(-2);
        var exception = Record.Exception(() =>
        {
            item.Modify("4");
        });

        // Assert
        Assert.Null(exception);


    }





}