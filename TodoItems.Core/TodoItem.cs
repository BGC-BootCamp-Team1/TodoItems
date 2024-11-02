using TodoItems.Core.ApplicationExcepetions;
using TodoItems.Core.DueDateSettingStrategy;
namespace TodoItems.Core;

public class TodoItem
{
    public string Id { get; init; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
    public DateTime CreatedTime { get; set; }
    public IList<Modification> Modifications { get; private set; }
    public DateTime DueDate { get; set; }
    public DueDateSettingOptions dueDateSettingOptions { get; set; }

    public TodoItem(string description, List<Modification> modifications, DateTime dueDate)
    {
        Id = Guid.NewGuid().ToString();
        Description = description;
        CreatedTime = DateTime.Now;
        Modifications = [.. modifications];
        DueDate = dueDate;
    }

    public TodoItem()
    {
    }

    public void Modify(string description)
    {
        DateTime today = DateTime.Today;
        int count = Modifications.Count(modification => modification.Timestamp.Date == today);
        if (count < Constants.MAX_DAILY_MODIFICATIONS)
        {
            Description = description;
            Modifications.Add(new Modification(DateTime.Now));
        }
        else {
            throw new ExceedMaxModificationException();
        }
    }
}

public class Modification
{
    public DateTime Timestamp { get; set; }
    public Modification(DateTime timestamp)
    {
        Timestamp = timestamp;
    }
}
