using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Permission
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }  // Liên kết với bảng User

    [BsonRepresentation(BsonType.ObjectId)]
    public string RoleId { get; set; }  // Liên kết với bảng Role
}
