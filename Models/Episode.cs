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

    [BsonRequired]
    [BsonElement("Title")]
    [JsonPropertyName("Title")]
    public string Title { get; set; } = null!;

    [BsonElement("Description")]
    [JsonPropertyName("Description")]
    [BsonDefaultValue("")]
    public string? Description { get; set; } = "";

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
    public int ShotsTotal { get; set; } = 0;

    [BsonElement("ShotsCompleted")]
    [JsonPropertyName("ShotsCompleted")]
    [BsonDefaultValue("0")]
    public int ShotsCompleted { get; set; } = 0;

    [BsonElement("Sequences")]
    [JsonPropertyName("Sequences")]
    [BsonDefaultValue("[]")]
    public List<Sequence> Sequences { get; set; } = new List<Sequence>();

    public void setId()
    {
        this.Id = ObjectId.GenerateNewId().ToString();
    }

    /*public Episode cloneEpisode(){
        Episode newEpisode = new Episode();

        newEpisode.Id = this.Id;
        newEpisode.Number = this.Number;
        newEpisode.Title = this.Title;
        newEpisode.Description = this.Description;
        newEpisode.Director = this.Director;
        newEpisode.Writer = this.Writer;
        newEpisode.ShotsTotal = this.ShotsTotal;
        newEpisode.ShotsCompleted = this.ShotsCompleted;

        for (int i = 0; i < this.Sequences.Count; i++)
        {
            Sequences[i] = this.Sequences[i].cloneSequence();
        }

        return newEpisode;
    }*/
}