using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Core.AppException;

namespace TodoItems.Core.Strategy
{
    public class MostAvailableInFiveDaysStrategy : ICreateTodoStrategy
    {
        public TodoItem Create(string description, DateOnly? dueDate, ITodosRepository todosRepository)
        {
            DateOnly mostAvailableInFiveDays = DateOnly.FromDateTime(DateTime.Today);
            int countTodosByMostAvailableInFiveDays = todosRepository.CountTodoItemsByDueDate(mostAvailableInFiveDays);
            for (int i = 1; i < 5; i++)
            {
                var preDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(i));
                var count = todosRepository.CountTodoItemsByDueDate(preDueDate);
                if (count < countTodosByMostAvailableInFiveDays)
                {
                    mostAvailableInFiveDays = preDueDate;
                    countTodosByMostAvailableInFiveDays = count;
                }
            }
            if (countTodosByMostAvailableInFiveDays < 8)
            {
                var newTodoItem = new TodoItem(description, mostAvailableInFiveDays);
                return newTodoItem;
            }
            throw new ExceedMaxTodoItemsPerDueDateException();
        }
    }
}
