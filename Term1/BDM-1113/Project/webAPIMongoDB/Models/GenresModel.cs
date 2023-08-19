using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace webAPIMongoDB.Models
{
    public class GenresModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

    }
}
