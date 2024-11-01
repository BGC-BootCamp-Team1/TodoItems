using TodoItems.Core.ApplicationExcepetions;

namespace TodoItems.Core
{
    public class TodoItemService
    {
        private readonly ITodosRepository _todosRepository;
        private readonly int _maxItemPerDueDate = 8;

        public TodoItemService(ITodosRepository repository)
        {
            _todosRepository = repository;
        }

        public TodoItem CreateItem(string description, DateTime dueDate) {
            if (dueDate <= DateTime.Now.Date)
            {
                throw new TooEarlyDueDateException();
            }
            if (_todosRepository.CountTodoItemsOnTheSameDueDate(dueDate)>= _maxItemPerDueDate)
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
