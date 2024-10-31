namespace TodoItems.Core
{
    public interface ITodosRepository
    {
        List<TodoItem> FindAllTodoItemsHaveTheSameDueDate(DateTime dueDate);
    }
}
