using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoItem.Infrastructure;

public class TodoItemPo{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
    public DateOnly DueDate { get; set; }
    public DateOnly CreateTime { get; set; }
    public TodoItems.Core.TodoItem? ConvertToTodoItem()
    {
        if (this == null) return null;
        return new TodoItems.Core.TodoItem(this.Description, this.DueDate)
        {
            Id = this.Id,
            CreateTime = this.CreateTime,
            IsComplete = this.IsComplete,
        };
    }
}