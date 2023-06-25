using CPM_backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CPM_backend.Services;

public class EpisodesService
{
    private readonly IMongoCollection<Project> _projectsCollection;

    public EpisodesService(IOptions<DatabaseConfiguration> databaseConfiguration)
    {
        var mongoClient = new MongoClient(databaseConfiguration.Value.Connection);

        var mongoDatabase = mongoClient.GetDatabase(databaseConfiguration.Value.Name);

        _projectsCollection = mongoDatabase.GetCollection<Project>(
            databaseConfiguration.Value.ProjectsCollection
        );
    }

    public async Task<List<Episode>> GetAsync(string idProject)
    {
        return await _projectsCollection
            .Find(Builders<Project>.Filter.Eq(p => p.Id, idProject))
            .Project(p => p.Episodes)
            .ToListAsync()
            .ContinueWith(t => t.Result.SelectMany(e => e).ToList());
    }

    public async Task<Episode?> GetAsync(string idProject, string idEpisode)
    {
        var episodes = await _projectsCollection
            .Find(Builders<Project>.Filter.Eq(p => p.Id, idProject))
            .Project(p => p.Episodes)
            .ToListAsync();

        var episode = episodes.SelectMany(e => e).FirstOrDefault(e => e.Id == idEpisode);

        return episode;
    }

    public async Task CreateAsync(string projectId, Episode episode)
    {
        var filter = Builders<Project>.Filter.Eq(p => p.Id, projectId);

        var project = await _projectsCollection.Find(filter).FirstOrDefaultAsync();
        if (project == null)
            return;

        if (project.Episodes != null)
        {
            if (project.Episodes.Count == 0)
                episode.Number = 1;
            else
                episode.Number = project.Episodes.Max(e => e.Number) + 1;
        }

        var update = Builders<Project>.Update.Push(p => p.Episodes, episode);

        await _projectsCollection.UpdateOneAsync(filter, update);
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

        await _projectsCollection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateAsync(
        string projectId,
        string episodeId,
        EpisodeUpdateDTO updatedEpisode
    )
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episodeId)
        );

        var episode = (
            await _projectsCollection.Find(filter).FirstOrDefaultAsync()
        )?.Episodes.FirstOrDefault(e => e.Id == episodeId);
        if (episode == null)
            return;

        var update = Builders<Project>.Update
            .Set(
                p => p.Episodes.FirstMatchingElement().Number,
                updatedEpisode.Number == 0 ? episode.Number : updatedEpisode.Number
            )
            .Set(
                p => p.Episodes.FirstMatchingElement().Title,
                updatedEpisode.Title ?? episode.Title
            )
            .Set(
                p => p.Episodes.FirstMatchingElement().Description,
                updatedEpisode.Description ?? episode.Description
            )
            .Set(
                p => p.Episodes.FirstMatchingElement().Writer,
                updatedEpisode.Writer ?? episode.Writer
            )
            .Set(
                p => p.Episodes.FirstMatchingElement().Director,
                updatedEpisode.Director ?? episode.Director
            );

        await _projectsCollection.UpdateOneAsync(filter, update);
    }
}
