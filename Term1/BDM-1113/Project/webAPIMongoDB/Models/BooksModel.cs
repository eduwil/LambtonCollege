using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Nodes;

namespace webAPIMongoDB.Models
{
    public class BooksModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement("ISBN")]
        public string ISBN { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("year")]
        public short year { get; set; }

        [BsonElement("author")]
        public List<string> author { get; set; }

        [BsonElement("genre")]
        public List<string> genre { get; set; }

        [BsonElement("pages")]
        public short pages { get; set; }

        [BsonElement("synopsis")]
        public string synopsis { get; set; }

        [BsonElement("languages")]
        public List<string> languages { get; set; }

        [BsonElement("related")]
        //public List<JsonArray> related { get; set; }
        //public IEnumerable<RelatedBook> related { get; set; }
        public List<RelatedBook> related { get; set; }
    }

    public class RelatedBook 
    {
        [BsonElement("ISBN")]
        public string ISBN { get; set; }

        [BsonElement("name")]
        public string name { get; set; }
    }

    public class BooksRateModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement("ISBN")]
        public string ISBN { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("year")]
        public short year { get; set; }

        [BsonElement("author")]
        public List<string> author { get; set; }

        [BsonElement("genre")]
        public List<string> genre { get; set; }

        [BsonElement("pages")]
        public short pages { get; set; }

        [BsonElement("synopsis")]
        public string synopsis { get; set; }

        [BsonElement("languages")]
        public List<string> languages { get; set; }

        [BsonElement("related")]
        public List<RelatedBook> related { get; set; }

        [BsonElement("bookRate")]
        public List<UserBooksModel> bookRate { get; set; }
    }

}
