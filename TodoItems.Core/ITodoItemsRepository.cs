namespace TodoItems.Core
{
    public interface ITodoItemsRepository
    {
        public Task<long> CountTodoItemsOnTheSameDueDate(DateTime dueDate);
        public Task<TodoItem> FindById(string id);
        void Save(TodoItem todoItem);
    }
}
