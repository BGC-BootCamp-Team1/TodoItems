using Microsoft.VisualBasic;
using MongoDB.Driver;
using TodoItem.Infrastructure;
using TodoItems.Core;


string connectionString = "mongodb://localhost:27017";
string databaseName = "TodoTestStore";
string todoItemsCollectionName = "Todos";

var client = new MongoClient(connectionString);
var database = client.GetDatabase(databaseName);
var todoItemsCollection = database.GetCollection<TodoItemPo>(todoItemsCollectionName);



var Modifications3TimesInOneDay = new List<Modification>
{
    new TodoItems.Core.Modification(DateTime.Today.AddHours(9)),  // 今天的上午9点
    new Modification(DateTime.Today.AddHours(14)), // 今天的下午2点
    new Modification(DateTime.Today.AddHours(18))  // 今天的下午6点
};

var todoItemPo = new TodoItemPo
{
    //Id = "5f9a7d8e2d3b4a1eb8a7d855",
    Description = "meal",
    ModificationTimestamps = Modifications3TimesInOneDay,
    DueDate = DateTime.Today.AddDays(1)
};
await todoItemsCollection.InsertOneAsync(todoItemPo);

var result = await todoItemsCollection.FindAsync(_ => true); 

foreach(var item in result.ToList())
{
    Console.WriteLine(item.Description, item.ModificationTimestamps, item.DueDate);
}

