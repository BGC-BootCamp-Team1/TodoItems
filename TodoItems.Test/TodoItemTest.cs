using TodoItems.Core;
using System;

namespace TodoItems.Test;

public class TodoItemTest
{
    [Fact]
    public void AddTimestamp_ShouldNotAddTimestamp_WhenMoreThanThreeTimestampsToday()

    {
        // Arrange

        var ModificationsTest = new List<Modification>
        {
            new Modification(DateTime.Now),
            new Modification(DateTime.Now.AddSeconds(1)),
            new Modification(DateTime.Now.AddSeconds(2))
        };

        var todoItem = new TodoItem("1", "Initial Description", ModificationsTest);


        // Act and Assert
        var exception = Assert.Throws<ArgumentException>(() => todoItem.ModifyDescription("bbb"));
        Assert.Equal("Initial Description", todoItem.Description);
        Assert.Equal(3, todoItem.ModificationTimestamps.Count);
        Assert.Equal(ModificationsTest, todoItem.ModificationTimestamps);


        // Verify the exception message
        Assert.Equal("You have reached the maximum number of modifications for today. Please try agian tomorrow.", exception.Message);
    }

    [Fact]
    public void AddTimestamp_ShouldAddTimestamp_WhenLessThanThreeTimestampsToday()
    {
        // Arrange
        var ModificationsInOneDay = new List<Modification>
        {
            new Modification(DateTime.Now),
        };

        var todoItem = new TodoItem("1", "Initial Description", ModificationsInOneDay);
        //Act
        todoItem.ModifyDescription("bbb");
        //Assert
        Assert.Equal("bbb", todoItem.Description);
        Assert.Equal(2, todoItem.ModificationTimestamps.Count);
        var lastTimestamp = todoItem.ModificationTimestamps.Last().ModificationTimestamp;
        Assert.Equal(DateTime.Now.Date, lastTimestamp.Date);
    }


    [Fact]
    public void AddTimestamp_ShouldAddTimestamp_WhenLessThanThreeTimestampsInOneToday()
    {
        // Arrange
        var ModificationsOnDifferentDay = new List<Modification>
        {
            new Modification(DateTime.Now),
            new Modification(DateTime.Now.AddSeconds(1)),
            new Modification(DateTime.Now.AddDays(-1)),
            new Modification(DateTime.Now.AddDays(-1).AddSeconds(-30))
        };

        

        var todoItem = new TodoItem("1", "Initial Description", ModificationsOnDifferentDay);
        //Act
        todoItem.ModifyDescription("tt");
        //Assert
        Assert.Equal("tt", todoItem.Description);
        Assert.Equal(5, todoItem.ModificationTimestamps.Count);
        var lastTimestamp = todoItem.ModificationTimestamps.Last().ModificationTimestamp;
        Assert.Equal(DateTime.Now.Date, lastTimestamp.Date);
    }


}