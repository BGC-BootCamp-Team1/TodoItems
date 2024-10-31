using System.Security.AccessControl;

namespace TodoItems.Core;

public class TodoItem
{
    public string Id { get; init; }
    public string Type { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedTime { get; private set; }
    public List<Modification> ModificationRecords { get; private set; }

    public TodoItem(string description, string type)
    {
        this.Description = description;
        this.Type = type;
        this.Id = Guid.NewGuid().ToString();
        this.CreatedTime = DateTime.Now;
        this.ModificationRecords = new List<Modification>();
    }

    public void  ModifyItem(string description, string type)
    {
        var todayModifications = ModificationRecords.Where(r => r.time.Date == DateTime.Today).ToList();
        if (todayModifications.Count < 3 )
        {
            this.Description = description;
            this.Type = type;
            this.ModificationRecords.Add(new Modification());
        }
        else
        {
            string errMsg = "You have reached the maximum number of modifications for today. Please try agian tomorrow.";
            throw new MaxModificationsReachedException(errMsg);
        }
    }
}
