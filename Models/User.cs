using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

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
    [BsonElement("Password")]
    [JsonPropertyName("Password")]
    public string Password { get; set; } = null!;

    public User cloneUser()
    {
        User newUser = new User();

        newUser.Id = this.Id;
        newUser.Username = this.Username;
        newUser.Password = this.Password;

        return newUser;
    }
}
