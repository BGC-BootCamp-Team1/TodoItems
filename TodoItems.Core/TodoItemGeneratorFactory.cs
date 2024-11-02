using TodoItems.Core.Repository;
using TodoItems.Core.Strategy;

namespace TodoItems.Core;

public class TodoItemGeneratorFactory
{
    private readonly ITodoItemsRepository _repository;

    public TodoItemGeneratorFactory(ITodoItemsRepository repository)
    {
        _repository = repository;
    }

    public ITodoItemGenerator GetGenerator(OptionEnum option, DateTime? dueDay)
    {
        if (dueDay.HasValue)
        {
            return new ManualTodoItemGenerator(_repository);
        }

        return option switch
        {
            OptionEnum.Latest => new LatestTodoItemGenerator(_repository),
            OptionEnum.Freest => new FreestTodoItemGenerator(_repository),
            _ => throw new ArgumentException("Invalid option")
        };
    }
}
