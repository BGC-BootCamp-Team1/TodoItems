using TodoItems.Core.AppException;
using TodoItems.Core.Model;
using TodoItems.Core.Repository;

namespace TodoItems.Core.Service
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodosRepository _repository;

        public TodoItemService(ITodosRepository repository)
        {
            _repository = repository;
        }

        public TodoItem Create(string description, DateOnly dueDay, string userId)
        {
            if(dueDay < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new DueDayEarlyException($"cannot earlier than today");
            }

            List<TodoItem> items= _repository.FindAllTodoItemsByUserIdAndDueDay(userId, dueDay);
                
            if (items.Count >= Constants.MAX_DAY_SAME_DUEDAY) 
            {
                throw new MaximumSameDueDayException($"too many items in same day for {userId}");
            }

            TodoItem todoItem = new TodoItem(description, dueDay, userId); ;
            _repository.Insert(todoItem);
            return todoItem;

        }

    }
}
