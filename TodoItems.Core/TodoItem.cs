using System.Security.AccessControl;

namespace TodoItems.Core;

public class TodoItem
{
    public DateTimeOffset DueDate { get; set; }
    public string Id { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreateTime { get; set; }
    public List<Modification> ModificationHistory { get; set; }
    public TodoItem(string description, DateTimeOffset dueDate)
    {
        Id = Guid.NewGuid().ToString();
        Description = description;
        CreateTime = DateTimeOffset.Now;
        ModificationHistory = new List<Modification>();
        DueDate = dueDate;
    }
    public void ModifyItem(string modifiedDescription)
    {
        if (ModificationHistory.Count(m => m.TimesStamp.Date == DateTime.Today) < 3)
        {
            ModificationHistory.Add(new Modification());
            Description = modifiedDescription;
        }
        else
            throw new Exception("Modification Limit.");

    }
}

