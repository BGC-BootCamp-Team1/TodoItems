using TodoItems.Core.Model;
using TodoItems.Core.Repository;

namespace TodoItems.Core.Strategy;

public class FreestTodoItemGenerator : ITodoItemGenerator
{
    private readonly ITodoItemsRepository _repository;

    public FreestTodoItemGenerator(ITodoItemsRepository repository)
    {
        _repository = repository;
    }

    public TodoItem Generate(string description, DateTime? dueDay, string userId)
    {
        var todoItems = _repository.FindTodoItemsInFiveDaysByUserId(userId);
        var dueDayList = todoItems
            .GroupBy(item => item.DueDay)
            .Select(group => new { DueDay = group.Key, Count = group.Count() })
            .OrderBy(item => item.Count)
            .ToList();

        foreach (var pair in dueDayList)
        {
            if (pair.Count < Constants.MAX_DAY_SAME_DUEDAY)
            {
                return new TodoItem(description, (DateTime)pair.DueDay, userId);
            }
        }

        throw new MaximumSameDueDayException("too many items on the same day");
    }
}
