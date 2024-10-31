using System;

namespace TodoItems.Core;

public class TodoItem
{
    public string Id { get; set; } 
    public string Description { get; set; }
    public IList<Modification> Modifications { get; set; }

    private readonly int _maxNumberOfModification = 3;

    public void Modify(string description)
    {
        int count = Modifications.Count(modification => modification.TimeStamp.Date == DateTime.Today);
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
    public DateTime TimeStamp { get; set; }
    public Modification(DateTime timeStamp)
    {
        TimeStamp = timeStamp;
    }
}
