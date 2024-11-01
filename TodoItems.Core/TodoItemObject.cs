
namespace TodoItems.Core;

public class TodoItemObject
{
    public string Id { get; init; }
    public string Description { get; private set; }
    public List<Modification> ModificationTimestamps { get; set; }
    public DateTime? DueDate { get; init; }


    public TodoItemObject(string description, DateTime dueDate)
    {
        Id = Guid.NewGuid().ToString();
        Description = description;
        ModificationTimestamps = new List<Modification>();
        DueDate = dueDate;
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
        else
        {
            this.Description = newDescription;
            this.ModificationTimestamps.Add(new Modification(DateTime.Now));
        }
    }
}
