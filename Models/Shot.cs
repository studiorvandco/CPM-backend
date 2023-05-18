using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPM_backend.Models;

public class Shot
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("id")]
    public string Id { get; } = ObjectId.GenerateNewId().ToString();

    [BsonRequired]
    [BsonElement("Number")]
    [JsonPropertyName("number")]
    public int Number { get; internal set; }

    [BsonRequired]
    [BsonElement("Title")]
    [JsonPropertyName("title")]
    [JsonRequired]
    public string Title { get; set; } = null!;

    [BsonElement("Value")]
    [JsonPropertyName("value")]
    [BsonDefaultValue("")]
    public string Value { get; set; } = "";

    [BsonElement("Description")]
    [JsonPropertyName("description")]
    [BsonDefaultValue("")]
    public string Description { get; set; } = "";

    [BsonElement("Completed")]
    [JsonPropertyName("completed")]
    [BsonDefaultValue("false")]
    public bool Completed { get; set; } = false;
}

public class ShotUpdateDTO
{
    [JsonPropertyName("number")] public int Number { get; set; }

    [JsonPropertyName("title")] public string? Title { get; set; }

    [JsonPropertyName("value")] public string? Value { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("completed")] public bool? Completed { get; set; }
}