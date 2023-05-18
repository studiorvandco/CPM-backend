using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPM_backend.Models;

public class Episode
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("id")]
    public string Id { get; } = ObjectId.GenerateNewId().ToString();

    [BsonRequired]
    [BsonElement("Number")]
    [JsonPropertyName("number")]
    public int Number { get; internal set; } = 0;

    [BsonRequired]
    [JsonRequired]
    [BsonElement("Title")]
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [BsonElement("Description")]
    [JsonPropertyName("description")]
    [BsonDefaultValue("")]
    public string Description { get; set; } = "";

    [BsonElement("Director")]
    [JsonPropertyName("director")]
    [BsonDefaultValue("")]
    public string Director { get; set; } = "";

    [BsonElement("Writer")]
    [JsonPropertyName("writer")]
    [BsonDefaultValue("")]
    public string Writer { get; set; } = "";

    [BsonElement("ShotsTotal")]
    [JsonPropertyName("shots_total")]
    [BsonDefaultValue("0")]
    public int ShotsTotal { get; internal set; } = 0;

    [BsonElement("ShotsCompleted")]
    [JsonPropertyName("shots_completed")]
    [BsonDefaultValue("0")]
    public int ShotsCompleted { get; internal set; } = 0;

    [BsonElement("Sequences")]
    [JsonPropertyName("sequences")]
    [BsonDefaultValue("[]")]
    [JsonIgnore]
    public List<Sequence> Sequences { get; } = new();
}

public class EpisodeUpdateDTO
{
    [JsonPropertyName("number")]
    public int Number { get; set; } = 0;

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("director")]
    public string? Director { get; set; }

    [JsonPropertyName("writer")]
    public string? Writer { get; set; }
}
