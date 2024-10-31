
namespace TodoItems.Core;

public class TodoItem
{
    private string id;
    public string Description { get; set; }
    public List<Modification> ModificationTimestamps;



    public TodoItem(string id, string description, List<Modification> modificationTimestamps)
    {
        this.id = id;
        this.Description = description;
        this.ModificationTimestamps = modificationTimestamps;
    }

    

    public void ModifyDescription(string newDescription)
    {
        const int MaximumNumberInOneDay = 3;
        const string ErrorMessage = "You have reached the maximum number of modifications for today. Please try agian tomorrow.";

        DateTime today = DateTime.Today;
        int todayCount = this.ModificationTimestamps.Count(ts => ts.ModificationTimestamp.Date == today);

        if (todayCount >= MaximumNumberInOneDay)
        {
            throw new ArgumentException(ErrorMessage);
        }
        else {
            this.Description = newDescription;
            this.ModificationTimestamps.Add(new Modification(DateTime.Now));
        }
    }
}
