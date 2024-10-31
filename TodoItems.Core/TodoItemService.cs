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
                throw new DueDateEarlierThanCreationDateException("Cannot create todo item that due date earlier than creation date");
            }

            var itemCount = _todosRepository.GetCountByDueDate(dueDate);

            if (itemCount >= MaxItemsPerDueDate)
            {
                throw new MaxItemsPerDueDateReachedException($"Cannot create new Todo item completed on {dueDate}, already reach max limit({MaxItemsPerDueDate})");
            }
            _todosRepository.Create(newItem);
            return newItem;
        }

    }
}
