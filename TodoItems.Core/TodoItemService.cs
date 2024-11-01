using Microsoft.VisualBasic;
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

        public TodoItem Create(string description, DateTime? manualSetDueDate, DueDateSetStrategy strategy = DueDateSetStrategy.Manual)
        {
            return TodoItemFactory.CreateItem(_todosRepository, description, manualSetDueDate, strategy);
        }
        
    }
}
