using TodoItems.Core.BizException;
using TodoItems.Core.Model;
using TodoItems.Core.Repository;

namespace TodoItems.Core.Service
{
    public class TodoItemService
    {
        private const int MAX_DAY_SAME_DUEDAY = 8;
        public List<TodoItem> itemList;
        private ITodosRepository _repository;

        public TodoItemService(ITodosRepository repository)
        {
            itemList = new List<TodoItem>();
            _repository = repository;
        }

        public TodoItem Create(string description, DateOnly dueDay, string userId)
        {
            if(dueDay < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new NotificationException($"canot ealier than today");

            }
            int itemCount = itemList
                .Count(item => item.UserId == userId && item.DueDay == dueDay);

            if (itemCount >= MAX_DAY_SAME_DUEDAY) 
            {
                throw new NotificationException($"too many items in same day for {userId}");
            }

            TodoItem todoItem = new TodoItem(description, dueDay, userId); ;
            itemList.Add(todoItem);
            return todoItem;

        }

    }
}
