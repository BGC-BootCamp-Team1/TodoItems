namespace TodoItems.Core;

public class TodoItemService
{
    private ITodosRepository _todosRepository;
    public TodoItemService(ITodosRepository todosRepository)
    {
        TodoItems = new List<TodoItem>();
        _todosRepository = todosRepository;
    }
    public List<TodoItem> TodoItems { get; set; }
    public TodoItem Create(string description,
                           DateOnly dueDate,
                           CreateOption createOption)
    {
        if(dueDate != null)
            createOption = CreateOption.ManualOption;
        switch (createOption)
        {
            case CreateOption.ManualOption:
                var count = _todosRepository.CountTodoItemsByDueDate(dueDate);
                var isValidDueDate = dueDate >= DateOnly.FromDateTime(DateTime.Today);
                if (!isValidDueDate)
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
                break;
            case CreateOption.NextAvailableInFiveDaysOption:
                for (int i = 0; i< 5; i++)
                {
                    var preDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(i));
                    count = _todosRepository.CountTodoItemsByDueDate(preDueDate);
                    if (count < 8)
                    {
                        var newTodoItem = new TodoItem(description, dueDate);
                        TodoItems.Add(newTodoItem);
                        _todosRepository.SaveAsync(newTodoItem);
                        return newTodoItem;
                    }                        
                }
                throw new Exception("TodoItems count limit on dueDate");
                break;
            case CreateOption.MostAvailableInFiveDaysOption:
                DateOnly mostAvailableInFiveDays = DateOnly.FromDateTime(DateTime.Today);
                int CountTodosByMostAvailableInFiveDays = _todosRepository.CountTodoItemsByDueDate(mostAvailableInFiveDays);
                for (int i = 1; i<5; i++)
                {
                    var preDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(i));
                    count = _todosRepository.CountTodoItemsByDueDate(preDueDate);
                    if (count < CountTodosByMostAvailableInFiveDays) {
                        mostAvailableInFiveDays = preDueDate;
                        CountTodosByMostAvailableInFiveDays = count;
                    }
                }
                if (CountTodosByMostAvailableInFiveDays < 8)
                {
                    var newTodoItem = new TodoItem(description, dueDate);
                    TodoItems.Add(newTodoItem);
                    _todosRepository.SaveAsync(newTodoItem);
                    return newTodoItem;
                }
                throw new Exception("TodoItems count limit on dueDate");
                break;
            default:
                throw new Exception("Wrong create option");
                break;
        }

    }
    public enum CreateOption
    {
        ManualOption = 0,
        NextAvailableInFiveDaysOption,
        MostAvailableInFiveDaysOption
    }
}

