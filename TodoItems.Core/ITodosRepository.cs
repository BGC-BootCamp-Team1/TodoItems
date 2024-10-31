using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoItems.Core
{
    public interface ITodosRepository
    {
        TodoItem Create(TodoItem item);
        int GetCountByDueDate(DateTime date);
    }
}
