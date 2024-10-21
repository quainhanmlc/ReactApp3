using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ReactApp3.Server.Models
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; } // Liên kết tới User
        public string Content { get; set; }
        public string Link { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsSeen { get; set; }
    }

}
