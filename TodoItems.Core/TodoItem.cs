
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

    public class TodoItemTimestamps
    {
        
    }

    public void AddTimestamp()
    {
        DateTime now = DateTime.Now;
        DateTime today = now.Date;
        const int MaximumNumberInOneDay = 3;

        
        int todayCount = ModificationTimestamps.Count(ts => ts.ModificationTimestamp.Date == today);

       
        if (todayCount >= MaximumNumberInOneDay)
        {
            return;
        }

    }


    public void ModifyDescription(string newDescription)
    {  
        const string ErrorMessage = "You have reached the maximum number of modifications for today. Please try agian tomorrow.";
        throw new ArgumentException(ErrorMessage);
        
    }
}
