using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using BC = BCrypt.Net.BCrypt;

namespace CPM_backend.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("id")]
    public string Id { get; } = ObjectId.GenerateNewId().ToString();

    [BsonRequired]
    [BsonElement("Username")]
    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [BsonRequired]
    [JsonIgnore]
    [BsonElement("Hash")]
    public string Hash { get; private set; } = null!;

    [BsonIgnore]
    public string Password
    {
        set => Hash = BC.HashPassword(value);
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