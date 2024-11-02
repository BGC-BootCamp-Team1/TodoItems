using TodoItems.Core.Model;
using TodoItems.Core.Repository;

namespace TodoItems.Core.Strategy;

public class ManualTodoItemGenerator : ITodoItemGenerator
{
    private readonly ITodoItemsRepository _repository;

    public ManualTodoItemGenerator(ITodoItemsRepository repository)
    {
        _repository = repository;
    }

    public TodoItem Generate(string description, DateTime? dueDay, string userId)
    {
        if (dueDay is null || dueDay < DateTime.Today)
        {
            throw new DueDayEarlyException("Due date cannot be earlier than today");
        }

        var items = _repository.FindAllTodoItemsByUserIdAndDueDay(userId, dueDay.Value);
        if (items.Count >= Constants.MAX_DAY_SAME_DUEDAY)
        {
            throw new MaximumSameDueDayException($"too many items on the same day for {userId}");
        }

        return new TodoItem(description, dueDay.Value, userId);
    }
}
