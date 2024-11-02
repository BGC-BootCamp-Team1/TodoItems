
namespace TodoItems.Core;

public interface ITodoRepository
{
    public Task<TodoItemObject> FindById(string id);
    void Save(TodoItemObject todoItem);
    public int CountTodoItemsOnDueDate(DateTime dueDate);
    
}
