using TodoItems.Core.ApplicationExcepetions;

namespace TodoItems.Core.DueDateSettingStrategy
{
    public class FirstAvailableDayStrategy : IDueDateSettingStrategy
    {
        public DateTime GetDueDate(DateTime startDate, List<TodoItem> itemsDueInNextFiveDays)
        {
            for (int i = 0; i < 5; i++)
            {
                var targetDate = startDate.AddDays(i).Date;
                var itemCount = itemsDueInNextFiveDays.Count(item => item.DueDate.Date == targetDate);
                if (itemCount < Constants.MAX_ITEM_SAME_DUEDAY)
                {
                    return targetDate;
                }
            }
            throw new NoAvailableDaysException();
        }
    }
}
