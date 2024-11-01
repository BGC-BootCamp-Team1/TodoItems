using TodoItems.Core.ApplicationException;
using static TodoItems.Core.Constants;

namespace TodoItems.Core
{
    public class TodoItemService
    {
        private readonly ITodosRepository _todosRepository;

        public TodoItemService(ITodosRepository todosRepository)
        {
            _todosRepository = todosRepository;
        }

        public TodoItem Create(string description, DateTime dueDate, DueDateSetStrategy strategy)
        {
            var newItem = new TodoItem(description, dueDate);

            if (dueDate < newItem.CreatedTime)
            {
                throw new DueDateEarlierThanCreationDateException();
            }

            var itemCount = _todosRepository.GetCountByDueDate(dueDate);

            if (itemCount >= MAX_ITEMS_PER_DUE_DATE)
            {
                throw new MaxItemsPerDueDateReachedException(dueDate, MAX_ITEMS_PER_DUE_DATE);
            }
            _todosRepository.Create(newItem);
            return newItem;
        }

    }
}
