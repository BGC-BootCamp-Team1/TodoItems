
namespace TodoItems.Core;

public interface ITodoRepository
{
    public Task<TodoItemObject> FindById(string id);
    public Task Save(TodoItemObject todoItem);
    public int CountTodoItemsOnDueDate(DateTime dueDate);
    
}
