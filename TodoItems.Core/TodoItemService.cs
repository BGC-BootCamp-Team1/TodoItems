using TodoItems.Core.ApplicationExcepetions;

namespace TodoItems.Core
{
    public class TodoItemService
    {
        private readonly ITodosRepository _todosRepository;

        public TodoItemService(ITodosRepository repository)
        {
            _todosRepository = repository;
        }

        public TodoItem CreateItem(string description, DateTime dueDate) {
            if (dueDate <= DateTime.Now.Date)
            {
                throw new TooEarlyDueDateException();
            }
            if (_todosRepository.CountTodoItemsOnTheSameDueDate(dueDate)>= Constants.MAX_ITEM_SAME_DUEDAY)
            {
                throw new ExceedMaxTodoItemsPerDueDateException();
            }
            TodoItem item = new TodoItem
            {
                Id = Guid.NewGuid().ToString(),
                Description = description,
                Modifications = [],
                DueDate = dueDate
            };
            return item;
        }
    }
}
