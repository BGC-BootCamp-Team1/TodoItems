namespace TodoItems.Core;

public class TodoItem
{
    public string _id;
    public string Description {  get; set; }
    public List<Modification> ModificationList { get; set; }

    public TodoItem(string description)
    {
        _id = Guid.NewGuid().ToString();
        Description = description;
        ModificationList = new List<Modification>();
    }

    public TodoItem() : this("")
    {
        
    }

    public void Modify(string description)
    {
        int count = 0;
        ModificationList.ForEach(modification =>
        {
            if (DateTime.Now.Subtract(modification.TimeStamp).TotalDays <= 1)
            {
                count++;
            }
        });
        if (count >= 3) {
            throw new NotificationException("You have reached the maximum number of modifications for today. Please try agian tomorrow.");
        }

        if (!description.Equals(Description))
        {
            Description = description;
            ModificationList.Add(new Modification());
        }
    }
}
