using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

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

    [BsonRequired]
    [BsonElement("isFilm")]
    [JsonPropertyName("isFilm")]
    public Boolean isFilm { get; set; }

    [BsonRequired]
    [BsonElement("isSeries")]
    [JsonPropertyName("isSeries")]
    public Boolean isSeries { get; set; }

    [BsonElement("Episodes")]
    [JsonPropertyName("Episodes")]
    [BsonDefaultValue("[]")]
    public List<Episode> Episodes { get; set; } = new List<Episode>();

    public Project WithDefaults() {
        this.Title ??= "";
        this.Description ??= "";
        this.BeginDate ??= DateTimeOffset.MinValue;
        this.EndDate ??= DateTimeOffset.MinValue;
        return this;
    }

}
