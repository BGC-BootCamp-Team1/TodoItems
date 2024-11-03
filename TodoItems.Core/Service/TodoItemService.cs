using TodoItems.Core.Model;
using TodoItems.Core.Repository;

namespace TodoItems.Core.Service
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemsRepository _repository;

        public TodoItemService(ITodoItemsRepository repository)
        {
            _repository = repository;
        }

        public TodoItem Create(OptionEnum option, string description, DateTime? dueDay, string userId)
        {
            var generator = new TodoItemGeneratorFactory(_repository).GetGenerator(option, dueDay);
            var todoItem = generator.Generate(description, dueDay, userId);
            return _repository.Save(todoItem);
        }

    }
}
