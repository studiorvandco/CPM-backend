using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Title")]
    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [BsonElement("Description")]
    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [BsonElement("BeginDate")]
    [JsonPropertyName("BeginDate")]
    public DateTimeOffset? BeginDate { get; set; }

    [BsonElement("EndDate")]
    [JsonPropertyName("EndDate")]
    public DateTimeOffset? EndDate { get; set; }

    [BsonRequired]
    [BsonElement("ShotsTotal")]
    [JsonPropertyName("ShotsTotal")]
    public int ShotsTotal { get; set; }

    [BsonRequired]
    [BsonElement("ShotsCompleted")]
    [JsonPropertyName("ShotsCompleted")]
    public int ShotsCompleted { get; set; }

    [BsonElement("isFilm")]
    [JsonPropertyName("isFilm")]
    public Boolean? isFilm { get; set; }

    [BsonElement("isSerie")]
    [JsonPropertyName("isSerie")]
    public Boolean? isSerie { get; set; }

    [BsonElement("Episodes")]
    [JsonPropertyName("Episodes")]
    public List<Episode>? Episodes { get; set; }

    public Project cloneProject()
    {
        Project newProject = new Project();

        newProject.Id = this.Id;
        newProject.Title = this.Title;
        newProject.Description = this.Description;
        newProject.BeginDate = this.BeginDate;
        newProject.EndDate = this.EndDate;
        newProject.ShotsTotal = this.ShotsTotal;
        newProject.ShotsCompleted = this.ShotsCompleted;
        newProject.isFilm = this.isFilm;
        newProject.isSerie = this.isSerie;
        newProject.Episodes = this.Episodes;

        if (this.Episodes == null)
            newProject.Episodes = null;
        else
            for (int i = 0; i < this.Episodes.Count; i++)
            {
                Episodes[i] = this.Episodes[i].cloneEpisode();
            }

        return newProject;
    }
}
