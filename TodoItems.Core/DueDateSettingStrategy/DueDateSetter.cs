using Microsoft.VisualBasic;
using TodoItems.Core.ApplicationExcepetions;

namespace TodoItems.Core.DueDateSettingStrategy
{
    public class DueDateSetter
    {
        public DueDateSetter()
        {
        }

        public DateTime AutoSetDueDate(
            List<TodoItem> itemsDueInNextFiveDays, 
            DueDateSettingOption dueDateSettingOption)
        {
            var strategy = SetStrategy(dueDateSettingOption);
            return strategy.GetDueDate(DateTime.Today, itemsDueInNextFiveDays);
        }

        public DateTime ValidUserDueDate(DateTime userProvidedDueDate, long count)
        {
            if (userProvidedDueDate <= DateTime.Now.Date)
            {
                throw new TooEarlyDueDateException();
            }
            if (count >= Constants.MAX_ITEM_SAME_DUEDAY)
            {
                throw new ExceedMaxTodoItemsPerDueDateException();
            }
            return userProvidedDueDate;
        }

        
        private IDueDateSettingStrategy SetStrategy(DueDateSettingOption dueDateSettingOption)
        {
            return dueDateSettingOption switch
            {
                DueDateSettingOption.SelectFirstAvailableDay => new FirstAvailableDayStrategy(),
                DueDateSettingOption.SelectFewestTodoItemsDay => new FewestTodoItemsDayStrategy(),
                _ => throw new ArgumentException("Invalid strategy type"),
            };
        }
    }
}
