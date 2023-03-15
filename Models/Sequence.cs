using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Sequence
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("Number")]
    [JsonPropertyName("Number")]
    public int Number { get; set; } = 0;

    [BsonRequired]
    [BsonElement("Title")]
    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [BsonElement("Description")]
    [JsonPropertyName("Description")]
    [BsonDefaultValue("")]
    public string? Description { get; set; }

    [BsonRequired]
    [BsonElement("BeginDate")]
    [JsonPropertyName("BeginDate")]
    public DateTimeOffset? BeginDate { get; set; }

    [BsonRequired]
    [BsonElement("EndDate")]
    [JsonPropertyName("EndDate")]
    public DateTimeOffset? EndDate { get; set; }

    [BsonElement("ShotsTotal")]
    [JsonPropertyName("ShotsTotal")]
    [BsonDefaultValue("0")]
    public int ShotsTotal { get; set; } = 0;

    [BsonElement("ShotsCompleted")]
    [JsonPropertyName("ShotsCompleted")]
    [BsonDefaultValue("0")]
    public int ShotsCompleted { get; set; } = 0;

    [BsonElement("Shots")]
    [JsonPropertyName("Shots")]
    [BsonDefaultValue("[]")]
    public List<Shot> Shots { get; set; } = new List<Shot>();

    public Sequence WithDefaults() {
        this.Id = ObjectId.GenerateNewId().ToString();
        this.Title ??= "";
        this.Description ??= "";
        this.BeginDate ??= DateTimeOffset.MinValue;
        this.EndDate ??= DateTimeOffset.MinValue;
        return this;
    }

}