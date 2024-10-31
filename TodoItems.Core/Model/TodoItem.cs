using System.Security.Cryptography;
using TodoItems.Core.BizException;
namespace TodoItems.Core.Model;

public class TodoItem
{
    public string _id;
    public string Description { get; set; }
    public DateOnly DueDay { get; set; }
    public string UserId { get; set; }
    public List<Modification> ModificationList { get; set; }

    public TodoItem(string description, DateOnly dueDay, string userId)
    {
        _id = Guid.NewGuid().ToString();
        Description = description;
        DueDay = dueDay;
        UserId = userId;
        ModificationList = new List<Modification>();
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
        if (count >= Constants.MAX_MODIFY_TIME_ONE_DAY)
        {
            throw new MaximumModificationException("You have reached the maximum number of modifications for today. Please try agian tomorrow.");
        }

        if (!description.Equals(Description))
        {
            Description = description;
            ModificationList.Add(new Modification());
        }
    }
}
