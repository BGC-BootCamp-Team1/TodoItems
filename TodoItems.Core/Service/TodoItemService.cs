using TodoItems.Core.Model;
using TodoItems.Core.Repository;

namespace TodoItems.Core.Service
{
    public class TodoItemService
    {
        private List<TodoItem> todoItems;
        private ITodosRepository _repository;

        public TodoItemService(ITodosRepository repository)
        {
            todoItems = new List<TodoItem>();
            _repository = repository;
        }

        public TodoItem Create(string description, DateOnly dueDay, string userId)
        {
            return null;
            
        }


    }
}
