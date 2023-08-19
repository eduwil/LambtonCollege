using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace webAPIMongoDB.Models
{
    public class UsersModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement("nickname")]
        public string nickname { get; set; }

        [BsonElement("birth_year")]
        public short birth_year { get; set; }

        [BsonElement("genre")]
        public string genre { get; set; }

        [BsonElement("interests")]
        public List<string> interests { get; set; }

    }
}
