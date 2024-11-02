using TodoItems.Core.AppException;

namespace TodoItems.Core;

public partial class TodoItemService
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
                           CreateOptionEnum createOption)
    {
        if(dueDate != null)
            createOption = CreateOptionEnum.ManualOption;
        switch (createOption)
        {
            case CreateOptionEnum.ManualOption:
                var count = _todosRepository.CountTodoItemsByDueDate(dueDate);
                var isValidDueDate = dueDate >= DateOnly.FromDateTime(DateTime.Today);
                if (!isValidDueDate)
                    throw new InvalidDueDateException();
                if (count < 8)
                {
                    var newTodoItem = new TodoItem(description, dueDate);
                    TodoItems.Add(newTodoItem);
                    return newTodoItem;
                }
                else
                {
                    throw new ExceedMaxTodoItemsPerDueDateException();
                }
                break;
            case CreateOptionEnum.NextAvailableInFiveDaysOption:
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
                throw new ExceedMaxTodoItemsPerDueDateException();
                break;
            case CreateOptionEnum.MostAvailableInFiveDaysOption:
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
                throw new ExceedMaxTodoItemsPerDueDateException();
                break;
            default:
                throw new InvalidCreateOption();
                break;
        }

    }
}

