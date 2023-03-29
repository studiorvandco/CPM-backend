using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; private set; }

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
    [JsonRequired]
    [BsonElement("BeginDate")]
    [JsonPropertyName("BeginDate")]
    public DateTimeOffset BeginDate { get; set; } = DateTimeOffset.MinValue;

    [BsonRequired]
    [JsonRequired]
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

    [BsonRequired]
    [JsonRequired]
    [BsonElement("isFilm")]
    [JsonPropertyName("isFilm")]
    public Boolean isFilm { get; set; }

    [BsonRequired]
    [JsonRequired]
    [BsonElement("isSeries")]
    [JsonPropertyName("isSeries")]
    public Boolean isSeries { get; set; }

    [BsonElement("Episodes")]
    [JsonPropertyName("Episodes")]
    [BsonDefaultValue("[]")]
    public List<Episode> Episodes { get; private set; } = new List<Episode>();

}

public class ProjectUpdateDTO
{
    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("BeginDate")]
    public DateTimeOffset? BeginDate { get; set; }

    [JsonPropertyName("EndDate")]
    public DateTimeOffset? EndDate { get; set; }

}