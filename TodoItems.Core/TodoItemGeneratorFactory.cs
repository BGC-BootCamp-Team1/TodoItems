using TodoItems.Core.Repository;
using TodoItems.Core.Strategy;

namespace TodoItems.Core;

public class TodoItemGeneratorFactory(ITodoItemsRepository repository)
{
    public ITodoItemGenerator GetGenerator(OptionEnum option, DateTime? dueDay)
    {
        if (dueDay.HasValue)
        {
            return new ManualTodoItemGenerator(repository);
        }

        return option switch
        {
            OptionEnum.Latest => new LatestTodoItemGenerator(repository),
            OptionEnum.Freest => new FreestTodoItemGenerator(repository),
            _ => throw new ArgumentException("Invalid option")
        };
    }
}
