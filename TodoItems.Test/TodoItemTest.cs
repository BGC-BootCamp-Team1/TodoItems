using TodoItems.Core.AppException;
using TodoItems.Core.Model;

namespace TodoItems.Test;
public class TodoItemTest
{

    [Fact]
    public void should_return_todoItem_when_create()
    {
        TodoItem item = new TodoItem("Des", DateTime.Today,"user1");
        Assert.NotNull(item.Id);
        Assert.NotNull(item.Description);
        Assert.NotNull(item.ModificationList);
    }
    [Fact]
    public void should_contain_1_modification_when_modify()
    {
        TodoItem item = new TodoItem("Des", DateTime.Today, "user1");
        item.Modify("newDes");
        Assert.Single(item.ModificationList);
    }

    [Fact]
    public void should_throw_exception_when_more_than_3_today()
    {
        TodoItem item = new TodoItem("Des", DateTime.Today, "user1");
        item.Modify("1");
        item.Modify("2");
        item.Modify("3");

        Assert.Throws<MaximumModificationException>(() =>
        {
            item.Modify("4");
        });
    }

    [Fact]
    public void should_not_throw_exception_when_less_than_3_today()
    {
        TodoItem item = new TodoItem("Des", DateTime.Today, "user1");
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