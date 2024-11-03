using TodoItems.Core.Model;
using TodoItems.Core.Repository;

namespace TodoItems.Core.Strategy;

public class FreestTodoItemGenerator(ITodoItemsRepository repository) : ITodoItemGenerator
{
    public TodoItem Generate(string description, DateTime? dueDay, string userId)
    {
        var todoItems = repository.FindTodoItemsInFiveDaysByUserId(userId);
        var dueDayList = todoItems
            .GroupBy(item => item.DueDay)
            .Select(group => new { DueDay = group.Key, Count = group.Count() })
            .OrderBy(item => item.Count);
        
        foreach (var dueDayCountPair in dueDayList)
        {
            if (dueDayCountPair.Count < Constants.MAX_DAY_SAME_DUEDAY)
            {
                return new TodoItem(description, (DateTime)dueDayCountPair.DueDay, userId);
            }
        }

        throw new MaximumSameDueDayException("too many items on the same day");
    }
}
