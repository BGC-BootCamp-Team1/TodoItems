using TodoItems.Core.ApplicationExcepetions;
namespace TodoItems.Core;

public class TodoItem
{
    public string Id { get; init; }
    public string Description { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public IList<Modification> Modifications { get; set; }
    public DateTime DueDate { get; set; }

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
