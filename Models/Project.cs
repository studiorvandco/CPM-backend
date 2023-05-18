using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPM_backend.Models;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("id")]
    public string Id { get; } = ObjectId.GenerateNewId().ToString();

    [BsonRequired]
    [JsonRequired]
    [BsonElement("Title")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [BsonElement("Description")]
    [JsonPropertyName("description")]
    [BsonDefaultValue("")]
    public string Description { get; set; } = "";

    [BsonRequired]
    [JsonRequired]
    [BsonElement("BeginDate")]
    [JsonPropertyName("begin_date")]
    public DateTimeOffset BeginDate { get; set; } = DateTimeOffset.MinValue;

    [BsonRequired]
    [JsonRequired]
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

    [BsonRequired]
    [JsonRequired]
    [BsonElement("IsMovie")]
    [JsonPropertyName("is_movie")]
    public bool IsMovie { get; set; }

    [BsonElement("Episodes")]
    [JsonPropertyName("episodes")]
    [BsonDefaultValue("[]")]
    [JsonIgnore]
    public List<Episode> Episodes { get; } = new();
}

public class ProjectUpdateDTO
{
    [JsonPropertyName("title")] public string? Title { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("begin_date")] public DateTimeOffset? BeginDate { get; set; }

    [JsonPropertyName("end_date")] public DateTimeOffset? EndDate { get; set; }
}