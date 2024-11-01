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

        public TodoItem Create(OptionEnum option,string description, DateTime? dueDay, string userId)
        {
            TodoItem todoItem = null;
            if (dueDay is null)
            {
                if (option == OptionEnum.OptionA)
                {
                    todoItem = GenerateByOptionA(description, userId);
                }
                else if (option == OptionEnum.OptionB) 
                {
                    todoItem = GenerateByOptionB(description, dueDay, userId);
                }
            }
            else
            {
                todoItem = GenerateByManul(description, dueDay, userId);
            }

            _repository.Insert(todoItem);
            return todoItem;

        }

        private TodoItem GenerateByOptionB(string description, DateTime? dueDay, string userId)
        {
            List<TodoItem> todoItems = _repository.FindTodoItemsInFiveDaysByUserId(userId);
            var list = todoItems
                        .GroupBy(item => item.DueDay)
                        .Select(group => new { DueDay = group.Key, Count = group.Count() })
                        .OrderBy(item => item.Count)
                        .ToList();

            foreach (var pair in list)
            {
                if (pair.Count < Constants.MAX_DAY_SAME_DUEDAY)
                {
                    return new TodoItem(description, (DateTime)pair.DueDay, userId); ;
                }
            }
            throw new MaximumSameDueDayException("to many dueDay in same day");
        }

        private TodoItem GenerateByOptionA(string description, string userId)
        {
            List<TodoItem> todoItems = _repository.FindTodoItemsInFiveDaysByUserId(userId);
            var list = todoItems
                        .GroupBy(item => item.DueDay)
                        .Select(group => new { DueDay = group.Key, Count = group.Count() })
                        .OrderBy(item => item.DueDay)
                        .ToList();
            foreach (var pair in list)
            {
                if (pair.Count < Constants.MAX_DAY_SAME_DUEDAY)
                {
                    return new TodoItem(description, (DateTime)pair.DueDay, userId); ;
                }
            }
            throw new MaximumSameDueDayException("to many dueDay in same day");
        }

        private TodoItem GenerateByManul(string description, DateTime? dueDay, string userId)
        {
            if (dueDay < DateTime.Today)
            {
                throw new DueDayEarlyException($"cannot earlier than today");
            }

            List<TodoItem> items = _repository.FindAllTodoItemsByUserIdAndDueDay(userId, (DateTime)dueDay);

            if (items.Count >= Constants.MAX_DAY_SAME_DUEDAY)
            {
                throw new MaximumSameDueDayException($"too many items in same day for {userId}");
            }
            TodoItem todoItem = new TodoItem(description, (DateTime)dueDay, userId); ;
            return todoItem;
        }

    }
}
