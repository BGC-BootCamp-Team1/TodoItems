using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoItem.Infrastructure;

public class TodoStoreDatabaseSettings
{
    public required string ConnectionString { get; set; }

    public required string DatabaseName { get; set; }

    public required string TodoItemsCollectionName { get; set; }
}
