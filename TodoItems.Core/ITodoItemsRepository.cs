namespace TodoItems.Core
{
    public interface ITodoItemsRepository
    {
        int CountTodoItemsOnTheSameDueDate(DateTime dueDate);
        public Task<TodoItem> FindById(string id);
        void Save(TodoItem todoItem);
    }
}
