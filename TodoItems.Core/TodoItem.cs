namespace TodoItems.Core;
using static TodoItems.Core.Constants;

public class TodoItem
{
    public string Id { get; init; }
    public DateTime? DueDate { get; init; }
    public string Description { get; set; }
    public DateTime CreatedTime { get; init; }
    public List<Modification> ModificationRecords { get; private set; }

    public TodoItem()
    {
        this.ModificationRecords = new List<Modification>();
        this.CreatedTime = DateTime.Now;
    }

    public TodoItem(string description, DateTime? dueDate)
    {
        this.Description = description;
        this.Id = Guid.NewGuid().ToString();
        this.DueDate = dueDate;
        this.CreatedTime = DateTime.Now;
        this.ModificationRecords = new List<Modification>();
    }

    public void ModifyItem(string description)
    {
        var today = DateTime.Today;
        var todayModifications = ModificationRecords.Where(r => r.time.Date == today).ToList();
        if (todayModifications.Count < MAX_MODIFY_TIME_ONE_DAY)
        {
            this.Description = description;
            this.ModificationRecords.Add(new Modification());
        }
        else
        {
            throw new MaxModificationsReachedException();
        }
    }
}
