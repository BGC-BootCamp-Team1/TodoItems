using Microsoft.VisualBasic;

namespace TodoItems.Core;

public class TodoItemService
{
    private readonly ITodoRepository _repository;
    public List<TodoItemObject> TodoItems { get; private set; }
    public enum DuedateType
    {
        None = 0,
        FirstAvailableDay = 1,
        FewestDayWithIn5Days = 2
    }

    public TodoItemService(ITodoRepository repository)
    {
        _repository = repository;
        TodoItems = new List<TodoItemObject>();
    }


    public DateTime? DetermineLastDueDate(DateTime? dueDate, DuedateType type)
    {
        DateTime? lastDueDate = null;

        if (dueDate == null)
        {
            if (type == DuedateType.None)
            {
                lastDueDate = null;
            }
            else if (type == DuedateType.FirstAvailableDay)
            {
                lastDueDate = FindFirstAvailableDayWithinNext5Days();
            }
            else if (type == DuedateType.FewestDayWithIn5Days)
            {
                lastDueDate = FindDayWithFewestTodoItemsWithinNext5Days();
            }
        }
        else
        {
            ValidateDueDate(dueDate.Value);

            EnsureTodoItemLimitNotExceeded(dueDate.Value);
            lastDueDate = dueDate;
        }

        return lastDueDate;
    }



    private DateTime? FindFirstAvailableDayWithinNext5Days()
    {

        for (int i = 1; i <= Constants.SystemSetDuedateScope; i++)
        {
            DateTime futureDate = DateTime.Now.AddDays(i);
            var count = _repository.CountTodoItemsOnDueDate(futureDate);
            if (count < Constants.MaxTodoItemsPerDay)
            {
                return futureDate;
            }
        }
        return null;
    }


    private DateTime? FindDayWithFewestTodoItemsWithinNext5Days()
    {

        DateTime? bestDate = null;
        int fewestItems = Constants.MaxTodoItemsPerDay;

        for (int i = 1; i <= Constants.SystemSetDuedateScope; i++)
        {
            DateTime futureDate = DateTime.Now.AddDays(i);
            var count = _repository.CountTodoItemsOnDueDate(futureDate);
            if (count < fewestItems)
            {
                fewestItems = count;
                bestDate = futureDate;
            }
        }
        return bestDate;
    }

    public TodoItemObject CreateTodoItem(string description, DateTime dueDate)
    {
        ValidateDueDate(dueDate);
        EnsureTodoItemLimitNotExceeded(dueDate);

        var todoItem = new TodoItemObject()
        {
            Description = description,
            DueDate = dueDate,
        };
        TodoItems.Add(todoItem);
        return todoItem;
    }


    private void EnsureTodoItemLimitNotExceeded(DateTime dueDate)
    {

        var count = _repository.CountTodoItemsOnDueDate(dueDate);
        if (count >= Constants.MaxTodoItemsPerDay)
        {
            ApplicationException.ThrowTodoItemLimitExceededException();
        }
        
    }

    private void ValidateDueDate(DateTime dueDate)
    {
        if (dueDate < DateTime.Now)
        {
            ApplicationException.ThrowDuedateInPastException();

        }
       
    }


}
