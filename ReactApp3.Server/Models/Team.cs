using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ReactApp3.Server.Models
{
    public class Team
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfMember { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; } 
        [BsonRepresentation(BsonType.ObjectId)]
        public string ManagerId { get; set; } // Liên kết tới User
    }

}
