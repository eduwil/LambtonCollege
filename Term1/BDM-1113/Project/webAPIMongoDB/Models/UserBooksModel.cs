using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Xml;
using Microsoft.Extensions.Primitives;

namespace webAPIMongoDB.Models
{
    public class UserBooksModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement("nickname")]
        public string nickname { get; set; }

        [BsonElement("ISBN")]
        public string ISBN { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("view_date")]
        public string view_date { get; set; }

        [BsonElement("reading_date")]
        public string reading_date { get; set; }

        [BsonElement("read_date")]
        public string read_date { get; set; }

        [BsonElement("state")]
        public string state { get; set; }

        [BsonElement("rate")]
        public short rate { get; set; }

        [BsonElement("comment")]
        public string comment { get; set; }

    }
}
