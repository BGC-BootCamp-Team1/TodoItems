using System.Security.AccessControl;

namespace TodoItems.Core;

public class TodoItem
{
    public string Id { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreateTime { get; set; }
    public List<Modification> ModificationHistory { get; set; }
    public TodoItem(string description)
    {
        Id = Guid.NewGuid().ToString();
        Description = description;
        CreateTime = DateTimeOffset.Now;
        ModificationHistory = new List<Modification>();
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

public class Modification
{
    public DateTimeOffset TimesStamp { get; set; }
    public Modification()
    {
        TimesStamp = DateTimeOffset.Now;
    }
}

public class User
{
    public List<TodoItem> TodoItems { get; set; }
    public User()
    {
        TodoItems = new List<TodoItem>();
    }
}

