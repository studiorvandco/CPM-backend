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

    [BsonElement("Title")]
    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [BsonElement("Value")]
    [JsonPropertyName("Value")]
    public string? Value { get; set; }

    [BsonElement("Description")]
    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [BsonElement("Completed")]
    [JsonPropertyName("Completed")]
    public Boolean Completed { get; set; }

    public Shot cloneShot(){
        Shot newShot = new Shot();

        newShot.Id = this.Id;
        newShot.Number = this.Number;
        newShot.Title = this.Title;
        newShot.Value = this.Value;
        newShot.Description = this.Description;
        newShot.Completed = this.Completed;

        return newShot;
    }

}