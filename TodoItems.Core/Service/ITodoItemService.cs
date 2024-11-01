using TodoItems.Core.Model;

namespace TodoItems.Core.Service;

public interface ITodoItemService
{
    TodoItem Create(string description, DateOnly? dueDay, string userId);
}