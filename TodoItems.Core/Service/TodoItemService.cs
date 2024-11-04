using TodoItems.Core.AppException;
using TodoItems.Core.Strategy;

namespace TodoItems.Core;

public partial class TodoItemService
{
    private ITodosRepository _todosRepository;
    private Dictionary<CreateOptionEnum,ICreateTodoStrategy> _strategies;
    public TodoItemService(ITodosRepository todosRepository)
    {
        _todosRepository = todosRepository;
        _strategies = new Dictionary<CreateOptionEnum, ICreateTodoStrategy>
        {
            { CreateOptionEnum.ManualOption, new ManualStrategy() },
            { CreateOptionEnum.NextAvailableInFiveDaysOption, new NextAvailableInFiveDaysStrategy() },
            { CreateOptionEnum.MostAvailableInFiveDaysOption, new MostAvailableInFiveDaysStrategy() }
        };
    }
    public async Task<TodoItem> CreateAsync(string description, DateOnly? dueDate, CreateOptionEnum createOption)
    {
        if (dueDate != null)
            createOption = CreateOptionEnum.ManualOption;

        var strategy = _strategies[createOption];
        var newTodoItem = strategy.Create(description, dueDate, _todosRepository);

/*        TodoItems.Add(newTodoItem);
*/        await _todosRepository.SaveAsync(newTodoItem);
        return newTodoItem;
    }
    public async Modify()
}

