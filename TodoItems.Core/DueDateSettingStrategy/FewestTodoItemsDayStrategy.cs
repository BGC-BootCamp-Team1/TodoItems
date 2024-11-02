namespace TodoItems.Core.DueDateSettingStrategy
{
    public class FewestTodoItemsDayStrategy : IDueDateSettingStrategy
    {
        public DateTime GetDueDate(DateTime startDate, List<TodoItem> existingItems)
        {
            DateTime leastItemsDay = startDate;
            int leastItemsCount = int.MaxValue;

            for (int i = 0; i < 5; i++)
            {
                var targetDate = startDate.AddDays(i).Date;
                var itemCount = existingItems.Count(item => item.DueDate.Date == targetDate);
                if (itemCount < leastItemsCount)
                {
                    leastItemsCount = itemCount;
                    leastItemsDay = targetDate;
                }
            }

            if (leastItemsCount < Constants.MAX_ITEM_SAME_DUEDAY)
            {
                return leastItemsDay;
            }

            throw new Exception("No available days in the next 5 days.");
        }
    }
}
