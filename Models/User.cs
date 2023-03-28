using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using BC = BCrypt.Net.BCrypt;

namespace CPMApi.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; private set; }

    [BsonRequired]
    [BsonElement("Username")]
    [JsonPropertyName("Username")]
    public string Username { get; set; } = null!;

    [BsonRequired]
    [JsonIgnore]
    [BsonElement("Hash")]
    public string Hash { get; set; } = null!;

    [BsonIgnore]
    public string Password {
        set {
            Hash = BC.HashPassword(value);
        }
    }
}

public class UserUpdateDTO
{
    [BsonIgnore]
    [JsonPropertyName("Username")]
    public string? Username { get; set; }

    [BsonIgnore]
    [JsonPropertyName("Password")]
    public string? Password { get; set; }
}