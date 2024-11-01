using TodoItems.Core.Model;

namespace TodoItems.Core.Repository
{
    public interface ITodoItemsRepository
    {
        List<TodoItem> FindAllTodoItemsByUserIdAndDueDay(string userId, DateTime dueDay);
        List<TodoItem> FindTodoItemsInFiveDaysByUserId(string userId);
        bool Insert(TodoItem todoItem);
    }
}
