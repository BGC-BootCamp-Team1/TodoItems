namespace TodoItems.Core.DueDateSettingStrategy
{
    public class FirstAvailableDayStrategy : IDueDateSettingStrategy
    {
        public DateTime GetDueDate(DateTime startDate, List<TodoItem> existingItems)
        {
            for (int i = 0; i < 5; i++)
            {
                var targetDate = startDate.AddDays(i).Date;
                var itemCount = existingItems.Count(item => item.DueDate.Date == targetDate);
                if (itemCount < Constants.MAX_ITEM_SAME_DUEDAY)
                {
                    return targetDate;
                }
            }
            throw new Exception("No available days in the next 5 days.");
        }
    }
}
