

using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoItems.Core
{
    public class TodoItemService
    {
        private readonly ITodosRepository _todosRepository;

        public TodoItemService(ITodosRepository todosRepository)
        {
            _todosRepository = todosRepository;
        }

        public int MaxItemsPerDueDay { get; init; } = 8;

        public TodoItem Create(string description, DateTime dueDate)
        {
            var itemCount = _todosRepository.GetCountByDueDate(dueDate);

            if (itemCount >= MaxItemsPerDueDay)
            {
                throw new Exception($"Cannot create new Todo item completed on {dueDate}, already reach max limit({MaxItemsPerDueDay})");
            }
            var newItem = new TodoItem(description, dueDate);
            _todosRepository.Create(newItem);
            return newItem;
        }

    }
}
