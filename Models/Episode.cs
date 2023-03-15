using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Episode
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

    [BsonElement("Director")]
    [JsonPropertyName("Director")]
    [BsonDefaultValue("")]
    public string? Director { get; set; }

    [BsonElement("Writer")]
    [JsonPropertyName("Writer")]
    [BsonDefaultValue("")]
    public string? Writer { get; set; }

    [BsonElement("ShotsTotal")]
    [JsonPropertyName("ShotsTotal")]
    [BsonDefaultValue("0")]
    public int ShotsTotal { get; set; } = 0;

    [BsonElement("ShotsCompleted")]
    [JsonPropertyName("ShotsCompleted")]
    [BsonDefaultValue("0")]
    public int ShotsCompleted { get; set; } = 0;

    [BsonElement("Sequences")]
    [JsonPropertyName("Sequences")]
    [BsonDefaultValue("[]")]
    public List<Sequence> Sequences { get; set; } = new List<Sequence>();

    public Episode WithDefaults() {
        this.Id = ObjectId.GenerateNewId().ToString();
        this.Title ??= "";
        this.Description ??= "";
        this.Director ??= "";
        this.Writer ??= "";
        return this;
    }

}