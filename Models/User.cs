using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using BC = BCrypt.Net.BCrypt;

namespace CPMApi.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("Username")]
    [JsonPropertyName("Username")]
    public string Username { get; set; } = null!;

    [BsonRequired]
    [JsonIgnore]
    [BsonElement("Hash")]
    public string Hash { get; set; } = null!;

    public void HashPassword(string password)
    {
        Hash = BC.HashPassword(password);
    }

    public UserOutDTO ToOutDTO()
    {
        return new UserOutDTO
        {
            Id = this.Id,
            Username = this.Username
        };
    }
}

public class UserOutDTO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("Username")]
    [JsonPropertyName("Username")]
    public string? Username { get; set; }
}

public class UserInDTO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonIgnore]
    [JsonRequired]
    [JsonPropertyName("Username")]
    public string? Username { get; set; }

    [BsonIgnore]
    [JsonRequired]
    [JsonPropertyName("Password")]
    public string? Password { get; set; }

    public User? ToUser()
    {
        if (this.Username == null || this.Password == null)
            return null;
        return new User
        {
            Id = this.Id,
            Username = this.Username,
            Hash = BC.HashPassword(this.Password)
        };
    }
}

public class UserUpdateDTO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonIgnore]
    [JsonPropertyName("Username")]
    public string? Username { get; set; }

    [BsonIgnore]
    [JsonPropertyName("Password")]
    public string? Password { get; set; }

    public string? GetHash()
    {
        if (this.Password == null)
            return null;
        return BC.HashPassword(this.Password);
    }
}