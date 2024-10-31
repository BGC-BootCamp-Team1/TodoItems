using System.Security.AccessControl;

namespace TodoItems.Core;

public class TodoItem
{
    public DateTimeOffset DueDate { get; set; }
    public string Id { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreateTime { get; set; }
    public List<Modification> ModificationHistory { get; set; }
    public TodoItem(string description, DateTimeOffset dueDate)
    {
        Id = Guid.NewGuid().ToString();
        Description = description;
        CreateTime = DateTimeOffset.Now;
        ModificationHistory = new List<Modification>();
        DueDate = dueDate;
    }
    public void ModifyItem(string modifiedDescription)
    {
        if (ModificationHistory.Count(m => m.TimesStamp.Date == DateTime.Today) < 3)
        {
            ModificationHistory.Add(new Modification());
            Description = modifiedDescription;
        }
        else
            throw new Exception("Modification Limit.");

    }
}

public class Modification
{
    public DateTimeOffset TimesStamp { get; set; }
    public Modification()
    {
        TimesStamp = DateTimeOffset.Now;
    }
}

public class User
{
    public List<TodoItem> TodoItems { get; set; }
    public User()
    {
        TodoItems = new List<TodoItem>();
    }
}

public class TodoItemService
{
    ITodosRepository _todosRepository;
    public TodoItemService(ITodosRepository todosRepository)
    {
        TodoItems = new List<TodoItem>();
        _todosRepository = todosRepository;
    }
    public List<TodoItem> TodoItems { get; set; }
    public TodoItem Create(string description, DateTimeOffset dueDate)
    {       
        var count = _todosRepository.CountTodoItemsByDueDate(dueDate);
        var isValidDueDate = dueDate.Date >= DateTime.UtcNow;
        if(!isValidDueDate)
            throw new Exception("Invalid dueDate");
        if (count < 8)
        {
            var newTodoItem = new TodoItem(description, dueDate);
            TodoItems.Add(newTodoItem);
            return newTodoItem;
        }
        else
        {
            throw new Exception("TodoItems count limit on dueDate");
        }
    }
}

