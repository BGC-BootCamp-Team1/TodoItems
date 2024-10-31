
namespace TodoItems.Core;

public class TodoItemService
{
    private readonly ITodoRepository _repository;
    public List<TodoItem> TodoItems { get; private set; }

    public TodoItemService(ITodoRepository repository)
    {
        _repository = repository;
        TodoItems = new List<TodoItem>();
    }

    public TodoItem CreateTodoItem(string description, DateTime dueDate)
    {
        if(dueDate < DateTime.Now)
        {
            throw new ArgumentException("Due date cannot be in the past.");
        }
        var count = _repository.CountTodoItemsOnDueDate(dueDate);
        if (count < 8) {
            var todoItem = new TodoItem(description, dueDate);

            TodoItems.Add(todoItem);
            return todoItem;
        }
        else
        {
            throw new ArgumentException("You have reached the maximum number of todo items for this due date.");
        }
        
    }

    
}
