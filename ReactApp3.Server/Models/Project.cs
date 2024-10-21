using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ReactApp3.Server.Models
{
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string ProjectName { get; set; }
        public string RequestDescriptionFromCustomer { get; set; }
        public DateTime StartDate { get; set; }
        public string Image { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string EmployeeId { get; set; } // Liên kết tới User
        public DateTime ExpectedEndDate { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }
        public string ProjectStatus { get; set; } // Enum
    }

}
