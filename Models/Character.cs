using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPM_backend.Models;

public class Character
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("FirstName")]
    [JsonPropertyName("first_name")]
    [BsonDefaultValue("")]
    public string FirstName { get; set; } = "";

    [BsonElement("LastName")]
    [JsonPropertyName("last_name")]
    [BsonDefaultValue("")]
    public string LastName { get; set; } = "";
}