using TodoItems.Core.Model;

namespace TodoItems.Core.Strategy;

public interface ITodoItemGenerator
{
    TodoItem Generate(string description, DateTime? dueDay, string userId);
}
