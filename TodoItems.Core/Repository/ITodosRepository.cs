using TodoItems.Core.Model;

namespace TodoItems.Core.Repository
{
    public interface ITodosRepository
    {
        List<TodoItem> FindAllTodoItemsInToday(string userId);
    }
}
