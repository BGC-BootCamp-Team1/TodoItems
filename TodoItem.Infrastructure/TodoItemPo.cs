using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TodoItems.Core;

namespace TodoItem.Infrastructure;

public class TodoItemPo{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
    public DateTime CreatedTime { get; set; }
    private IList<Modification> Modifications { get; set; }
    public DateTime DueDate { get; set; }
}