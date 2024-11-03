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
            var today = DateOnly.FromDateTime(DateTime.Today);
            var availableDate = Enumerable.Range(0, 5)
                .Select(i => today.AddDays(i))
                .FirstOrDefault(d => todosRepository.CountTodoItemsByDueDate(d) < Constant.MAX_TODOITEMS_PER_DUE_DATE);

            if (availableDate != default)
            {
                return new TodoItem(description, availableDate);
            }

            throw new ExceedMaxTodoItemsPerDueDateException();
        }
    }

}
