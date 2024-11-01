namespace TodoItems.Core
{
    public interface ITodosRepository
    {
        int CountTodoItemsOnTheSameDueDate(DateTime dueDate);
    }
}
