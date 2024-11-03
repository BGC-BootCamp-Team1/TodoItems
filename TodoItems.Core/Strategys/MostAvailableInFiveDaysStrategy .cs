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
            var today = DateOnly.FromDateTime(DateTime.Today);
            var days = Enumerable.Range(0, 5).Select(i => today.AddDays(i));

            var mostAvailableInFiveDays = days
                .Select(d => new { Date = d, Count = todosRepository.CountTodoItemsByDueDate(d) })
                .OrderBy(x => x.Count)
                .First();

            if (mostAvailableInFiveDays.Count < Constant.MAX_TODOITEMS_PER_DUE_DATE)
            {
                return new TodoItem(description, mostAvailableInFiveDays.Date);
            }

            throw new ExceedMaxTodoItemsPerDueDateException();
        }
    }

}
