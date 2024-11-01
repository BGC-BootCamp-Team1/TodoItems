using TodoItems.Core;
using System;

namespace TodoItems.Test;

public class TodoItemTest
{
    [Fact]
    public void ShouldNotAddTimestamp_WhenMoreThanThreeTimestampsToday()

    {
        // Arrange

        var Modifications3TimesInOneDay = new List<Modification>
        {
            new Modification(DateTime.Now.AddHours(1)),
            new Modification(DateTime.Now.AddHours(2)),
            new Modification(DateTime.Now.AddHours(3))
        };

        var todoItem = new TodoItems.Core.TodoItemObject("Initial Description", DateTime.Now);
        todoItem.ModificationTimestamps = Modifications3TimesInOneDay;



        // Act and Assert
        var exception = Assert.Throws<ArgumentException>(() => todoItem.ModifyDescription("bbb"));
        Assert.Equal("Initial Description", todoItem.Description);
        Assert.Equal(3, todoItem.ModificationTimestamps.Count);
        Assert.Equal(Modifications3TimesInOneDay, todoItem.ModificationTimestamps);


        // Verify the exception message
        Assert.Equal("You have reached the maximum number of modifications for today. Please try agian tomorrow.", exception.Message);
    }

    [Fact]
    public void ShouldAddTimestamp_WhenLessThanThreeTimestampsToday()
    {
        // Arrange
        var ModificationsOnceOneDay = new List<Modification>
        {
            new Modification(DateTime.Now.AddHours(-3)),
        };

        var todoItem = new TodoItemObject("Initial Description", DateTime.Now);
        todoItem.ModificationTimestamps = ModificationsOnceOneDay;
        //Act
        todoItem.ModifyDescription("bbb");
        //Assert
        Assert.Equal("bbb", todoItem.Description);
        Assert.Equal(2, todoItem.ModificationTimestamps.Count);
        var lastTimestamp = todoItem.ModificationTimestamps.Last().ModificationTimestamp;
        Assert.Equal(DateTime.Now.Date, lastTimestamp.Date);
    }


    [Fact]
    public void ShouldAddTimestamp_WhenLessThanThreeTimestampsInOneToday()
    {
        // Arrange
        var ModificationsOnDifferentDay = new List<Modification>
        {
            new Modification(DateTime.Now),
            new Modification(DateTime.Now.AddSeconds(1)),
            new Modification(DateTime.Now.AddDays(-1)),
            new Modification(DateTime.Now.AddDays(-1).AddSeconds(-30))
        };

        var todoItem = new TodoItemObject("Initial Description", DateTime.Now);
        todoItem.ModificationTimestamps = ModificationsOnDifferentDay;
        //Act
        todoItem.ModifyDescription("tt");

        //Assert
        Assert.Equal("tt", todoItem.Description);
        Assert.Equal(5, todoItem.ModificationTimestamps.Count);
        var lastTimestamp = todoItem.ModificationTimestamps.Last().ModificationTimestamp;
        Assert.Equal(DateTime.Now.Date, lastTimestamp.Date);
    }

    [Fact]
    public void ShouldNotAddTimestamp_WhenMoreThanThreeTimestampsInOneToday()
    {
        // Arrange
        var ModificationsOnDifferentDay = new List<Modification>
        {
            new Modification(DateTime.Now),
            new Modification(DateTime.Now.AddSeconds(1)),
            new Modification(DateTime.Now.AddSeconds(2)),
            new Modification(DateTime.Now.AddDays(-1)),
            new Modification(DateTime.Now.AddDays(-1).AddSeconds(-30))
        };

        var todoItem = new TodoItemObject("Initial Description", DateTime.Now.AddDays(10));
        todoItem.ModificationTimestamps = ModificationsOnDifferentDay;
        //Act
        var exception = Assert.Throws<ArgumentException>(() => todoItem.ModifyDescription("bbb"));


        //Assert
        Assert.Equal("Initial Description", todoItem.Description);
        Assert.Equal(5, todoItem.ModificationTimestamps.Count);
        Assert.Equal(ModificationsOnDifferentDay, todoItem.ModificationTimestamps);
        Assert.Equal("You have reached the maximum number of modifications for today. Please try agian tomorrow.", exception.Message);

    }




}