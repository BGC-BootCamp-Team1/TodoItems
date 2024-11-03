using TodoItems.Core.Model;

namespace TodoItems.Core.Service;

public interface ITodoItemService
{
    TodoItem Create(OptionEnum option, string description, DateTime? dueDay, string userId);
}