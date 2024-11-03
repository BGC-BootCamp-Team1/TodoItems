namespace TodoItems.Core
{
    public interface ITodosRepository
    {
        TodoItem Create(TodoItem item);
        int GetCountByDueDate(DateOnly date);
    }
}
