using System;

namespace TodoItems.Core;

public class TodoItem
{
    public string Id { get; init; }
    public string Description { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public IList<Modification> Modifications { get; set; }
    public DateTime DueDate { get; set; }

    private readonly int _maxNumberOfModification = 3;

    public void Modify(string description)
    {
        DateTime today = DateTime.Today;
        int count = Modifications.Count(modification => modification.Timestamp.Date == today);
        if (count < _maxNumberOfModification)
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
