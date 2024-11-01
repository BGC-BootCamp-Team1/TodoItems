using TodoItems.Core.ApplicationException;

namespace TodoItems.Core
{
    public class TodoItemService
    {
        private readonly ITodosRepository _todosRepository;

        public TodoItemService(ITodosRepository todosRepository)
        {
            _todosRepository = todosRepository;
        }

        public int MaxItemsPerDueDate { get; private set; } = 8;

        public TodoItem Create(string description, DateTime dueDate)
        {
            var newItem = new TodoItem(description, dueDate);

            if (dueDate < newItem.CreatedTime)
            {
                throw new DueDateEarlierThanCreationDateException();
            }

            var itemCount = _todosRepository.GetCountByDueDate(dueDate);

            if (itemCount >= MaxItemsPerDueDate)
            {
                throw new MaxItemsPerDueDateReachedException(dueDate, MaxItemsPerDueDate);
            }
            _todosRepository.Create(newItem);
            return newItem;
        }

    }
}
