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

        //todoItem.ModifyDescription("bbb");


        // Assert

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
        var ModificationsTest = new List<Modification>
        {
            new Modification(DateTime.Now),
           
        };

        var todoItem = new TodoItem("1", "Initial Description", ModificationsTest);
        //act
        todoItem.ModifyDescription("bbb");

        Assert.Equal("bbb", todoItem.Description);
        Assert.Equal(2, todoItem.ModificationTimestamps.Count);
        var lastTimestamp = todoItem.ModificationTimestamps.Last().ModificationTimestamp;
        Assert.Equal(DateTime.Now.Date, lastTimestamp.Date);


    }


}