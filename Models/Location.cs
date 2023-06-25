using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPM_backend.Models;

public class Location
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("Name")]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [BsonElement("Position")]
    [JsonPropertyName("position")]
    [BsonDefaultValue("")]
    public string Position { get; set; } = string.Empty;
}
