using TodoItems.Core;

namespace TodoItems.Test;

public class TodoItemTest
{
    [Fact]
    public void should_return_2_when_add_1_1()
    {
        
        Assert.Equal("1", todoItem.GetId());
    }
}