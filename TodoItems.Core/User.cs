namespace TodoItems.Core;

public class User
{
    public List<TodoItem> TodoItems { get; set; }
    public User()
    {
        TodoItems = new List<TodoItem>();
    }
}

