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
                                
    public async Task<Episode?> GetAsync(string idProject, string idEpisode) {
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
        if (episode.Number <= 0) {
            var project = _ProjectsCollection.Find(filter).First();
            if (project.Episodes.Count == 0) {
                episode.Number = 1;
            } else {
                episode.Number = project.Episodes.Max(e => e.Number) + 1;
            }
        }
        
        var update = Builders<Project>.Update.Push(p => p.Episodes, episode.WithDefaults());

        await _ProjectsCollection.UpdateOneAsync(filter, update);
    }


    public async Task RemoveAsync(string projectId, string episodeId)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episodeId)
        );

        var update = Builders<Project>.Update.PullFilter(p => p.Episodes, e => e.Id == episodeId);

        var result = await _ProjectsCollection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateAsync(string projectId, string episodeId, Episode updatedEpisode)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episodeId)
        );
        var project = await _ProjectsCollection.Find(filter).FirstOrDefaultAsync();
        var episode = project.Episodes.FirstOrDefault(e => e.Id == episodeId);

        if (project == null || episode == null)
            return;

        var update = Builders<Project>.Update
            .Set(p => p.Episodes.FirstMatchingElement().Title, updatedEpisode.Title ?? episode.Title)
            .Set(p => p.Episodes.FirstMatchingElement().Description, updatedEpisode.Description ?? episode.Description)
            .Set(p => p.Episodes.FirstMatchingElement().Writer, updatedEpisode.Writer ?? episode.Writer)
            .Set(p => p.Episodes.FirstMatchingElement().Director, updatedEpisode.Director ?? episode.Director);
        
        await _ProjectsCollection.UpdateOneAsync(filter, update);
    }

}
