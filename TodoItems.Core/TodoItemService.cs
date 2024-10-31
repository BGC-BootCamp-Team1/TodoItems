namespace TodoItems.Core;

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

