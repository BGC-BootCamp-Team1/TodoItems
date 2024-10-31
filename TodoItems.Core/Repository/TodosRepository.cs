using TodoItems.Core.Model;

namespace TodoItems.Core.Repository
{
    public class TodosRepository : ITodosRepository
    {
        public List<TodoItem> FindAllTodoItemsByUserIdAndDueDay(string userId, DateOnly dueDay)
        {
            throw new NotImplementedException();
        }

        public bool Insert(TodoItem todoItem)
        {
            throw new NotImplementedException();
        }
    }
}
