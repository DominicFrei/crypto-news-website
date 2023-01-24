using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CryptoNewsApp.Data
{
    public class News
    {
		// The attribute `BsonId` signals the MongoDB driver that this field should used to map the `_id` from the Atlas document.
		// Remember to use the type `ObjectId` here as well.
        [BsonId] public ObjectId Id { get; set; }

		// The two other fields in each news are `title` and `date`. Since the C# coding style differs from the Atlas naming style, we have to map them.
		// Thankfully there is another handy attribute to achieve this: `BsonElement`. It takes the document field's name and maps it to the classes field name.
        [BsonElement("title")] public String Title { get; set; }
        [BsonElement("date")] public DateTime Date { get; set; }
    }
}