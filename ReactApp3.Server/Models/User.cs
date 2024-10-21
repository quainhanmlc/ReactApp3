using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public string PasswordResetToken { get; set; }
    public DateTime? PasswordResetExpiry { get; set; }

    public string FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Avatar { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public string PhoneNumber { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public string? TeamId { get; set; }
    public bool IsActive { get; set; }
}
