using System.Security.AccessControl;

namespace TodoItems.Core;

public class TodoItem
{
    public bool IsComplete { get; set; }
    public DateOnly DueDate { get; set; }
    public string Id { get; set; }
    public string Description { get; set; }
    public DateOnly CreateTime { get; set; }
    public List<Modification> ModificationHistory { get; set; }
    public TodoItem(string description, DateOnly dueDate)
    {
        Id = Guid.NewGuid().ToString();
        Description = description;
        CreateTime = DateOnly.FromDateTime(DateTime.Today);
        ModificationHistory = new List<Modification>();
        DueDate = dueDate;
        IsComplete = false;
    }
    public void ModifyItem(string modifiedDescription)
    {
        if (ModificationHistory.Count(m => m.TimesStamp == DateOnly.FromDateTime(DateTime.Today)) < 3)
        {
            ModificationHistory.Add(new Modification());
            Description = modifiedDescription;
        }
        else
            throw new Exception("Modification Limit.");

    }
    public TodoItems.Core.TodoItem? ConvertToTodoItem()
    {
        if (this == null) return null;
        return new TodoItems.Core.TodoItem(this.Description, this.DueDate)
        {
            Id = this.Id,
            CreateTime = this.CreateTime,
            IsComplete = this.IsComplete,
        };
    }
}

