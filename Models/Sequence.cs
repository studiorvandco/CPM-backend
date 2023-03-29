using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Sequence
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = ObjectId.GenerateNewId().ToString();

    [BsonRequired]
    [BsonElement("Number")]
    [JsonPropertyName("Number")]
    public int Number { get; set; } = 0;

    [BsonRequired]
    [JsonRequired]
    [BsonElement("Title")]
    [JsonPropertyName("Title")]
    public string Title { get; set; } = null!;

    [BsonElement("Description")]
    [JsonPropertyName("Description")]
    [BsonDefaultValue("")]
    public string Description { get; set; } = "";

    [BsonRequired]
    [BsonElement("BeginDate")]
    [JsonPropertyName("BeginDate")]
    public DateTimeOffset BeginDate { get; set; } = DateTimeOffset.MinValue;

    [BsonRequired]
    [BsonElement("EndDate")]
    [JsonPropertyName("EndDate")]
    public DateTimeOffset EndDate { get; set; } = DateTimeOffset.MaxValue;

    [BsonElement("ShotsTotal")]
    [JsonPropertyName("ShotsTotal")]
    [BsonDefaultValue("0")]
    public int ShotsTotal { get; internal set; } = 0;

    [BsonElement("ShotsCompleted")]
    [JsonPropertyName("ShotsCompleted")]
    [BsonDefaultValue("0")]
    public int ShotsCompleted { get; internal set; } = 0;

    [BsonElement("Shots")]
    [JsonPropertyName("Shots")]
    [BsonDefaultValue("[]")]
    public List<Shot> Shots { get; private set; } = new List<Shot>();

}

public class SequenceUpdateDTO
{
    [JsonPropertyName("Number")]
    public int Number { get; set; } = 0;

    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("BeginDate")]
    public DateTimeOffset? BeginDate { get; set; }

    [JsonPropertyName("EndDate")]
    public DateTimeOffset? EndDate { get; set; }

}