using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Shot
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = ObjectId.GenerateNewId().ToString();

    [BsonRequired]
    [BsonElement("Number")]
    [JsonPropertyName("Number")]
    public int Number { get; set; }

    [BsonRequired]
    [BsonElement("Title")]
    [JsonPropertyName("Title")]
    [JsonRequired]
    public string Title { get; set; } = null!;

    [BsonElement("Value")]
    [JsonPropertyName("Value")]
    [BsonDefaultValue("")]
    public string Value { get; set; } = "";

    [BsonElement("Description")]
    [JsonPropertyName("Description")]
    [BsonDefaultValue("")]
    public string Description { get; set; } = "";

    [BsonElement("Completed")]
    [JsonPropertyName("Completed")]
    [BsonDefaultValue("false")]
    public Boolean Completed { get; set; } = false;

}

public class ShotUpdateDTO
{
    [JsonPropertyName("Number")]
    public int Number { get; set; }

    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [JsonPropertyName("Value")]
    public string? Value { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("Completed")]
    public Boolean? Completed { get; set; }

}