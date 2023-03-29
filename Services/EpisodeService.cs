using CPMApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CPMApi.Services;

public class EpisodesService
{
    private readonly IMongoCollection<Project> _ProjectsCollection;

    public EpisodesService(IOptions<DatabaseConfiguration> DatabaseConfiguration)
    {
        var mongoClient = new MongoClient(DatabaseConfiguration.Value.Connection);

        var mongoDatabase = mongoClient.GetDatabase(DatabaseConfiguration.Value.Name);

        _ProjectsCollection = mongoDatabase.GetCollection<Project>(
            DatabaseConfiguration.Value.ProjectsCollection
        );
    }

    public async Task<List<Episode>> GetAsync(string idProject) =>
        await _ProjectsCollection.Find(Builders<Project>.Filter.Eq(p => p.Id, idProject))
                                .Project(p => p.Episodes)
                                .ToListAsync()
                                .ContinueWith(t => t.Result.SelectMany(e => e).ToList());

    public async Task<Episode?> GetAsync(string idProject, string idEpisode)
    {
        var episodes = await _ProjectsCollection.Find(Builders<Project>.Filter.Eq(p => p.Id, idProject))
                                                .Project(p => p.Episodes)
                                                .ToListAsync();

        var episode = episodes.SelectMany(e => e)
                            .FirstOrDefault(e => e.Id == idEpisode);

        return episode;
    }

    public async Task CreateAsync(string projectId, Episode episode)
    {
        var filter = Builders<Project>.Filter.Eq(p => p.Id, projectId);

        var project = await _ProjectsCollection.Find(filter).FirstOrDefaultAsync();
        if (project == null) return;

        if (project.Episodes.Count == 0)
        {
            episode.Number = 1;
        }
        else
        {
            episode.Number = project.Episodes.Max(e => e.Number) + 1;
        }

        var update = Builders<Project>.Update.Push(p => p.Episodes, episode);

        await _ProjectsCollection.UpdateOneAsync(filter, update);
    }


    public async Task RemoveAsync(string projectId, Episode episode)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episode.Id)
        );

        var update = Builders<Project>.Update
            .PullFilter(p => p.Episodes, e => e.Id == episode.Id)
            .Inc(p => p.ShotsTotal, -episode.ShotsTotal)
            .Inc(p => p.ShotsCompleted, -episode.ShotsCompleted);

        var result = await _ProjectsCollection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateAsync(string projectId, string episodeId, EpisodeUpdateDTO updatedEpisode)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episodeId)
        );

        var episode = (await _ProjectsCollection.Find(filter).FirstOrDefaultAsync())
                            ?.Episodes.FirstOrDefault(e => e.Id == episodeId);
        if (episode == null) return;

        var update = Builders<Project>.Update
            .Set(p => p.Episodes.FirstMatchingElement().Number, updatedEpisode.Number == 0 ? episode.Number : updatedEpisode.Number)
            .Set(p => p.Episodes.FirstMatchingElement().Title, updatedEpisode.Title ?? episode.Title)
            .Set(p => p.Episodes.FirstMatchingElement().Description, updatedEpisode.Description ?? episode.Description)
            .Set(p => p.Episodes.FirstMatchingElement().Writer, updatedEpisode.Writer ?? episode.Writer)
            .Set(p => p.Episodes.FirstMatchingElement().Director, updatedEpisode.Director ?? episode.Director);

        await _ProjectsCollection.UpdateOneAsync(filter, update);
    }

}
