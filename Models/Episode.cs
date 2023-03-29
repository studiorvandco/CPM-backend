using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Episode
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

    [BsonElement("Director")]
    [JsonPropertyName("Director")]
    [BsonDefaultValue("")]
    public string Director { get; set; } = "";

    [BsonElement("Writer")]
    [JsonPropertyName("Writer")]
    [BsonDefaultValue("")]
    public string Writer { get; set; } = "";

    [BsonElement("ShotsTotal")]
    [JsonPropertyName("ShotsTotal")]
    [BsonDefaultValue("0")]
    public int ShotsTotal { get; internal set; } = 0;

    [BsonElement("ShotsCompleted")]
    [JsonPropertyName("ShotsCompleted")]
    [BsonDefaultValue("0")]
    public int ShotsCompleted { get; internal set; } = 0;

    [BsonElement("Sequences")]
    [JsonPropertyName("Sequences")]
    [BsonDefaultValue("[]")]
    public List<Sequence> Sequences { get; private set; } = new List<Sequence>();

}

public class EpisodeUpdateDTO
{
    [JsonPropertyName("Number")]
    public int Number { get; set; } = 0;

    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("Director")]
    public string? Director { get; set; }

    [JsonPropertyName("Writer")]
    public string? Writer { get; set; }

}