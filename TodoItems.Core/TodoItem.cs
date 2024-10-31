﻿namespace TodoItems.Core;

public class TodoItem
{
    public string Id { get; init; }
    public DateTime DueDate { get; init; }
    public string Description { get; private set; }
    public DateTime CreatedTime { get; init; }
    public List<Modification> ModificationRecords { get; private set; }

    public TodoItem(string description, DateTime dueDate)
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
        if (todayModifications.Count < 3)
        {
            this.Description = description;
            this.ModificationRecords.Add(new Modification());
        }
        else
        {
            string errMsg = "You have reached the maximum number of modifications for today. Please try agian tomorrow.";
            throw new MaxModificationsReachedException(errMsg);
        }
    }
}
