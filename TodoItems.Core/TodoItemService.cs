using Microsoft.VisualBasic;
using TodoItems.Core.ApplicationException;
using static TodoItems.Core.Constants;

namespace TodoItems.Core
{
    public class TodoItemService
    {
        private readonly ITodosRepository _todosRepository;

        public TodoItemService(ITodosRepository todosRepository)
        {
            _todosRepository = todosRepository;
        }

        public TodoItem Create(string description, DateTime? manualSetDueDate, DueDateSetStrategy strategy = DueDateSetStrategy.Manual)
        {
            var newItem = new TodoItem(description, manualSetDueDate);

            if (manualSetDueDate != null)
            {
                if (manualSetDueDate < newItem.CreatedTime.Date)
                {
                    throw new DueDateEarlierThanCreationDateException();
                }

                var itemCount = _todosRepository.GetCountByDueDate((DateTime)manualSetDueDate);

                if (itemCount >= MAX_ITEMS_PER_DUE_DATE)
                {
                    throw new MaxItemsPerDueDateReachedException((DateTime)manualSetDueDate, MAX_ITEMS_PER_DUE_DATE);
                }

                _todosRepository.Create(newItem);
                return newItem;
            }
            else if (strategy == DueDateSetStrategy.FirstDateOfNextFiveDays) 
            {
                var createdDate = newItem.CreatedTime.Date;
                for (var day = 0; day < 5; day++)
                {
                    var dueDate = createdDate.AddDays(day);

                    var itemCount = _todosRepository.GetCountByDueDate(dueDate);

                    if (itemCount < MAX_ITEMS_PER_DUE_DATE)
                    {
                        newItem.DueDate = dueDate;
                        return newItem;
                    }
                }
                throw new Exception("cannot find available date in next five days");
            }
            else if (strategy == DueDateSetStrategy.MostFreeDateOfNextFiveDays)
            {
                var createdDate = newItem.CreatedTime.Date;
                DateTime mostFreeDate = createdDate;
                int smallestCountPerDay = int.MaxValue;
                for (var day = 0; day < 5; day++)
                {
                    var dueDate = createdDate.AddDays(day);
                    var itemCount = _todosRepository.GetCountByDueDate(dueDate);
                    if (itemCount < smallestCountPerDay)
                    {
                        smallestCountPerDay = itemCount;
                        mostFreeDate = dueDate;
                    }
                }

                if (smallestCountPerDay >= MAX_ITEMS_PER_DUE_DATE)
                {
                    throw new Exception("cannot find available date in next five days");
                }
                newItem.DueDate = mostFreeDate;
            }

            return newItem;
            
        }
    }
}
