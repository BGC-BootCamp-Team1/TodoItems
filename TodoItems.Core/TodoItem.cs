﻿
namespace TodoItems.Core;

public class TodoItem
{
    public string Id { get; private set; }
    public string Description { get; private set; }
    public List<Modification> ModificationTimestamps { get;  set; }
    public DateTime? DueDate;


    public TodoItem(string description,DateTime dueDate)
    {
        this.Id = Guid.NewGuid().ToString();
        this.Description = description;
        this.ModificationTimestamps= new List<Modification>();
        this.DueDate = dueDate;
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
