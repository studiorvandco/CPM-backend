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
    [JsonPropertyName("username")]
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
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [BsonIgnore]
    [JsonPropertyName("password")]
    public string? Password { get; set; }
}