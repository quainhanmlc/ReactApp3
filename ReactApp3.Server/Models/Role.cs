using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ReactApp3.Server.Models
{
    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string RoleCode { get; set; }
        public string RoleName { get; set; }
    }

}
