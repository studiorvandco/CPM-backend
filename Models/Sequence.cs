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
    public int Number { get; set; }

    [BsonElement("Title")]
    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [BsonElement("Description")]
    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [BsonElement("Date")]
    [JsonPropertyName("Date")]
    public DateTimeOffset? Date { get; set; }

    [BsonElement("Time")]
    [JsonPropertyName("Time")]
    public string? Time { get; set; }

    [BsonElement("Shots")]
    [JsonPropertyName("Shots")]
    public List<Shot>? Shots { get; set; }

    public Sequence cloneSequence()
    {
        Sequence newSequence = new Sequence();

        newSequence.Id = this.Id;
        newSequence.Number = this.Number;
        newSequence.Title = this.Title;
        newSequence.Description = this.Description;
        newSequence.Date = this.Date;
        newSequence.Time = this.Time;
        if (this.Shots == null)
            newSequence.Shots = null;
        else
            for (int i = 0; i < this.Shots.Count; i++)
            {
                Shots[i] = this.Shots[i].cloneShot();
            }

        return newSequence;
    }
}