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
    public int Number { get; set; }

    [BsonElement("Title")]
    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [BsonElement("Description")]
    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [BsonElement("Director")]
    [JsonPropertyName("Director")]
    public string? Director { get; set; }

    [BsonElement("Writer")]
    [JsonPropertyName("Writer")]
    public string? Writer { get; set; }

    [BsonElement("Sequences")]
    [JsonPropertyName("Sequences")]
    public List<Sequence>? Sequences { get; set; }

    public Episode cloneEpisode()
    {
        Episode newEpisode = new Episode();

        newEpisode.Id = this.Id;
        newEpisode.Number = this.Number;
        newEpisode.Title = this.Title;
        newEpisode.Description = this.Description;
        newEpisode.Director = this.Director;
        newEpisode.Writer = this.Writer;
        if (this.Sequences == null)
            newEpisode.Sequences = null;
        else
            for (int i = 0; i < this.Sequences.Count; i++)
            {
                Sequences[i] = this.Sequences[i].cloneSequence();
            }

        return newEpisode;
    }
}