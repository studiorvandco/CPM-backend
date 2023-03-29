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
    public Boolean Completed { get; set; } = false;

}

public class ShotUpdateDTO
{
    [JsonPropertyName("number")]
    public int Number { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("completed")]
    public Boolean? Completed { get; set; }

}