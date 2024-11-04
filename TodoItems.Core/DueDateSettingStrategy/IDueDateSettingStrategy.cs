namespace TodoItems.Core.DueDateSettingStrategy
{
    public interface IDueDateSettingStrategy
    {
        public DateTime GetDueDate(DateTime startDate, List<TodoItem> itemsDueInNextFiveDays);
    }
}
