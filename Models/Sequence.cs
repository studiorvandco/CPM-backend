using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPM_backend.Models;

public class Sequence
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("id")]
    public string? Id { get; internal set; }

    [BsonRequired]
    [BsonElement("Number")]
    [JsonPropertyName("number")]
    public int Number { get; internal set; }

    [BsonRequired]
    [JsonRequired]
    [BsonElement("Title")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [BsonElement("Description")]
    [JsonPropertyName("description")]
    [BsonDefaultValue("")]
    public string Description { get; set; } = string.Empty;

    [BsonRequired]
    [BsonElement("BeginDate")]
    [JsonPropertyName("begin_date")]
    public DateTimeOffset BeginDate { get; set; } = DateTimeOffset.MinValue;

    [BsonRequired]
    [BsonElement("EndDate")]
    [JsonPropertyName("end_date")]
    public DateTimeOffset EndDate { get; set; } = DateTimeOffset.MaxValue;

    [BsonElement("ShotsTotal")]
    [JsonPropertyName("shots_total")]
    [BsonDefaultValue("0")]
    public int ShotsTotal { get; internal set; } = 0;

    [BsonElement("ShotsCompleted")]
    [JsonPropertyName("shots_completed")]
    [BsonDefaultValue("0")]
    public int ShotsCompleted { get; internal set; } = 0;

    [BsonElement("Shots")]
    [JsonPropertyName("shots")]
    [BsonDefaultValue("[]")]
    [JsonIgnore]
    public List<Shot> Shots { get; } = new();
}

public class SequenceUpdateDTO
{
    [JsonPropertyName("number")]
    public int Number { get; set; } = 0;

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("begin_date")]
    public DateTimeOffset? BeginDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTimeOffset? EndDate { get; set; }
}
