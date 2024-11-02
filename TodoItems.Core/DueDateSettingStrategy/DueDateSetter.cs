namespace TodoItems.Core.DueDateSettingStrategy
{
    public class DueDateSetter
    {
        private readonly IDueDateSettingStrategy _strategy;

        public DueDateSetter(IDueDateSettingStrategy strategy)
        {
            _strategy = strategy;
        }

        public DateTime SetDueDate(DateTime startDate, List<TodoItem> existingItems)
        {
            return _strategy.GetDueDate(startDate, existingItems);
        }
    }
}
