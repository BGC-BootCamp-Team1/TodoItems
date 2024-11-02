using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Core.AppException;

namespace TodoItems.Core.Strategy
{
    public class NextAvailableInFiveDaysStrategy : ICreateTodoStrategy
    {
        public TodoItem Create(string description, DateOnly? dueDate, ITodosRepository todosRepository)
        {
            for (int i = 0; i < 5; i++)
            {
                var preDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(i));
                var count = todosRepository.CountTodoItemsByDueDate(preDueDate);
                if (count < 8)
                {
                    var newTodoItem = new TodoItem(description, preDueDate);
                    return newTodoItem;
                }
            }
            throw new ExceedMaxTodoItemsPerDueDateException();
        }
    }
}
