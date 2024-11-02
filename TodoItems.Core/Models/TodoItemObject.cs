﻿
namespace TodoItems.Core;

public class TodoItemObject
{
    public string Id { get; init; }
    public string Description { get;  set; }
    public List<Modification> ModificationTimestamps { get; set; }
    public DateTime? DueDate { get; init; }


    public TodoItemObject()//(string description, DateTime dueDate)
    {
        Id = Guid.NewGuid().ToString();
        Description = "";
        ModificationTimestamps = new List<Modification>();
        DueDate = null;
    }



    public void ModifyDescription(string newDescription)
    {
       
        const string ErrorMessage = "You have reached the maximum number of modifications for today. Please try again tomorrow.";

        DateTime today = DateTime.Today;
        int todayCount = this.ModificationTimestamps.Count(ts => ts.ModificationTimestamp.Date == today);

        if (todayCount < Constants.MaxModifyPerDay)
        {
            this.Description = newDescription;
            this.ModificationTimestamps.Add(new Modification(DateTime.Now));
        }
        else
        {
            throw new ArgumentException(ErrorMessage);
        }
    }
}
