using System.Runtime.InteropServices.JavaScript;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CryptoNewsApp.Data
{
    public class News
    {
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("title")] public String Title { get; set; }
        [BsonElement("date")] public DateTime Date { get; set; }
    }
}