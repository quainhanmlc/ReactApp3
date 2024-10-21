using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ReactApp3.Server.Models
{
    public class RefreshToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; } // Liên kết tới User
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
        public DateTime CreateTime { get; set; }
    }

}
