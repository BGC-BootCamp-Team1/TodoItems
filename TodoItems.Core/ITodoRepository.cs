
namespace TodoItems.Core;

public interface ITodoRepository
{
    public int CountTodoItemsOnDueDate(DateTime dueDate);
    
}
