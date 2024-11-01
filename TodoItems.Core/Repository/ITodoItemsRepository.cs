using TodoItems.Core.Model;

namespace TodoItems.Core.Repository
{
    public interface ITodoItemsRepository
    {
        List<TodoItem> FindAllTodoItemsByUserIdAndDueDay(string userId,DateOnly dueDay);
        bool Insert(TodoItem todoItem);
    }
}
