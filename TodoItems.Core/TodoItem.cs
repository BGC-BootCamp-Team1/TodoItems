namespace TodoItems.Core;

public class TodoItem
{
    public string Id { get; init; }
    public string Type { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedTime { get; private set; }

    public TodoItem(string description, string type)
    {
        this.Description = description;
        this.Type = type;
        this.Id = Guid.NewGuid().ToString();
        this.CreatedTime = DateTime.Now;
    }

    public void EditItem(string description, string type)
    {
        this.Description = description;
        this.Type = type;
    }

}
