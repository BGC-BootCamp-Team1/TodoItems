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

    public void EditItem(string description, string type)
    {
        this.Description = description;
        this.Type = type;
        this.ModificationRecords.Add(new Modification());
    }

    public bool ModifyItem(string description, string type, out string errMsg)
    {
        var todayModifications = ModificationRecords.Where(r => r.time.Date == DateTime.Today).ToList();
        if (todayModifications.Count < 3 )
        {
            EditItem(description, type);
            errMsg = "";
            return true;
        }
        else
        {
            errMsg = "You have reached the maximum number of modifications for today. Please try agian tomorrow.";
            return false;
        }
    }
}
