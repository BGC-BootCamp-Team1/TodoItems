using TodoItems.Core;

namespace TodoItems.Test;

public class TodoItemTest
{
    [Fact]
    public void should_throw_exception_when_fourth_modification()
    {
        DateTime today = DateTime.Today;
        List<Modification> threeTodayModifications =
        [
            new Modification(today.AddHours(9)),
            new Modification(today.AddHours(12)),
            new Modification(today.AddHours(14))
        ];
        var todoItem = new TodoItem() {
            Id = "testItem",
            Description = "test",
            Modifications = threeTodayModifications.ToList()
        };
        Assert.Throws<ExceedMaxModificationException>(() => todoItem.Modify("TEST"));
    }

    [Fact]
    public void should_modify_when_third_modification()
    {
        DateTime today = DateTime.Today;
        List<Modification> twoTodayModifications =
        [
            new Modification(today.AddDays(-1).AddHours(12)),
            new Modification(today.AddHours(12))
        ];
        var todoItem = new TodoItem()
        {
            Id = "testItem",
            Description = "test",
            Modifications = twoTodayModifications.ToList()
        };
        todoItem.Modify("TEST");
        Assert.Equal(twoTodayModifications.Count + 1, todoItem.Modifications.Count);
    }

    [Fact]
    public void should_modify_when_first_modification()
    {
        List<Modification> emptyModifications = [];
        var todoItem = new TodoItem()
        {
            Id = "testItem",
            Description = "test",
            Modifications = emptyModifications.ToList()
        };
        todoItem.Modify("TEST");
        Assert.Equal(emptyModifications.Count + 1, todoItem.Modifications.Count);
    }
}