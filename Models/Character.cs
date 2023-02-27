using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Character
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("FirstName")]
    [JsonPropertyName("FirstName")]
    public string? FirstName { get; set; }

    [BsonElement("LastName")]
    [JsonPropertyName("LastName")]
    public string? LastName { get; set; }
}
