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

        public TodoItem Create(string description, DateTime? dueDate, DueDateSetStrategy strategy = DueDateSetStrategy.Manual)
        {
            var newItem = new TodoItem(description, dueDate);

            if (strategy == DueDateSetStrategy.Manual && dueDate != null)
            {
                if (dueDate < newItem.CreatedTime)
                {
                    throw new DueDateEarlierThanCreationDateException();
                }

                var itemCount = _todosRepository.GetCountByDueDate((DateTime)dueDate);

                if (itemCount >= MAX_ITEMS_PER_DUE_DATE)
                {
                    throw new MaxItemsPerDueDateReachedException((DateTime)dueDate, MAX_ITEMS_PER_DUE_DATE);
                }
                _todosRepository.Create(newItem);
                return newItem;
            }

            return newItem;
            
        }

    }
}
