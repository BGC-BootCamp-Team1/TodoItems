using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TodoItems.Core.Model;

namespace TodoItems.Infrastructure;
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

public static class TodoMapper
{
    public static TodoItemPo ToPo(TodoItem item)
    {
        return new TodoItemPo()
        {
            Id = item.Id,
            Description = item.Description,
            userId = item.UserId,
            DueDay = item.DueDay,
            ModificationList = item.ModificationList
        };
    }
    
    public static TodoItem ToItem(TodoItem po)
    {
        return new TodoItem(po.Id,po.Description,po.DueDay,po.UserId,po.ModificationList);
    }
    
    
    
}