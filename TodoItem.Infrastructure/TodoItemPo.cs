using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TodoItems.Core.Model;

namespace TodoItem.Infrastructure;
public class TodoItemPo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Description { get; set; }
    public string userId{ get; set; }
    public DateTime DueDay { get; set; }
    public List<Modification> ModificationList { get; set; }


}