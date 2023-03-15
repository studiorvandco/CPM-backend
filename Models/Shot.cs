using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Shot
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("Number")]
    [JsonPropertyName("Number")]
    public int Number { get; set; }

    [BsonRequired]
    [BsonElement("Title")]
    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [BsonElement("Value")]
    [JsonPropertyName("Value")]
    [BsonDefaultValue("")]
    public string? Value { get; set; }

    [BsonElement("Description")]
    [JsonPropertyName("Description")]
    [BsonDefaultValue("")]
    public string? Description { get; set; }

    [BsonElement("Completed")]
    [JsonPropertyName("Completed")]
    [BsonDefaultValue("false")]
    public Boolean Completed { get; set; } = false;

    public Shot WithDefaults() {
        this.Id = ObjectId.GenerateNewId().ToString();
        this.Title ??= "";
        this.Description ??= "";
        this.Value ??= "";
        return this;
    }

}