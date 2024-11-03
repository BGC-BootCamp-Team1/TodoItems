using TodoItems.Core.DueDateSettingStrategy;

namespace TodoItems.Core
{
    public class TodoItemService
    {
        private readonly ITodoItemsRepository _todosRepository;

        public TodoItemService(ITodoItemsRepository repository)
        {
            _todosRepository = repository;
        }

        public TodoItem CreateItem(string description, DateTime? userProvidedDueDate, DueDateSettingOption dueDateSettingOption = 0)
        {
            DateTime dueDate;
            if (userProvidedDueDate.HasValue)
            {
                var count = _todosRepository.CountTodoItemsOnTheSameDueDate(userProvidedDueDate.Value).Result;
                dueDate = DueDateSetter.ValidUserDueDate(userProvidedDueDate.Value, count);
            }
            else
            {
                var todoItemsDueInNextFiveDays = _todosRepository.GetTodoItemsDueInNextFiveDays().Result;
                dueDate = DueDateSetter.AutoSetDueDate(todoItemsDueInNextFiveDays, dueDateSettingOption);
            }
            TodoItem item = new TodoItem(description, [], dueDate);
            return item;
        }
    }
}
