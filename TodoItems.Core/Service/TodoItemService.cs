using TodoItems.Core.BizException;
using TodoItems.Core.Model;
using TodoItems.Core.Repository;

namespace TodoItems.Core.Service
{
    public class TodoItemService
    {
        public List<TodoItem> itemList;
        private ITodosRepository _repository;

        public TodoItemService(ITodosRepository repository)
        {
            itemList = new List<TodoItem>();
            _repository = repository;
        }

        public TodoItem Create(string description, DateOnly dueDay, string userId)
        {
            int itemCount = itemList
                .Count(item => item.UserId == userId && item.DueDay == dueDay);

            if (itemCount >= 8) 
            {
                throw new NotificationException($"too many items in same day for {userId}");
            }

            TodoItem todoItem = new TodoItem(description, dueDay, userId); ;
            itemList.Add(todoItem);
            return todoItem;

        }

    }
}
