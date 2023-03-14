using CPMApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CPMApi.Services;

public class ShotsService
{
    private readonly IMongoCollection<Project> _ProjectsCollection;

    public ShotsService(IOptions<DatabaseConfiguration> DatabaseConfiguration)
    {
        var mongoClient = new MongoClient(DatabaseConfiguration.Value.Connection);

        var mongoDatabase = mongoClient.GetDatabase(DatabaseConfiguration.Value.Name);

        _ProjectsCollection = mongoDatabase.GetCollection<Project>(
            DatabaseConfiguration.Value.ProjectsCollection
        );
    }

    public async Task<List<Shot>> GetAsync(string idProject, string idEpisode, string idSequence)
    {
        var project = await _ProjectsCollection.Find(p => p.Id == idProject).FirstOrDefaultAsync();

        if (project == null)
            return new List<Shot>();

        var episode = project.Episodes.FirstOrDefault(e => e.Id == idEpisode);

        if (episode == null)
            return new List<Shot>();

        var sequence = episode.Sequences.FirstOrDefault(s => s.Id == idSequence);

        if (sequence == null)
            return new List<Shot>();

        return sequence.Shots;
    }

    public async Task<Shot?> GetAsync(string idProject, string idEpisode, string idSequence, string idShot)
    {
        var episodes = await _ProjectsCollection.Find(Builders<Project>.Filter.Eq(p => p.Id, idProject))
                                                .Project(p => p.Episodes)
                                                .ToListAsync();

        var episode = episodes.SelectMany(e => e)
                            .FirstOrDefault(e => e.Id == idEpisode);

        var sequence = episode?.Sequences.FirstOrDefault(s => s.Id == idSequence);

        var shot = sequence?.Shots.FirstOrDefault(s => s.Id == idShot);

        return shot;
    }

    public async Task CreateAsync(string projectId, string episodeId, string sequenceId, Shot shot)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episodeId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes[0].Sequences, s => s.Id == sequenceId)
        );
        
        var test = _ProjectsCollection.Find(filter).First();
        Console.Write(test);

        var update = Builders<Project>.Update.Push(p => p.Episodes[0].Sequences[0].Shots, shot);

        await _ProjectsCollection.UpdateOneAsync(filter, update);
    }

    public async Task RemoveAsync(string projectId, string episodeId, string sequenceId, string shotId)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episodeId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes[0].Sequences, s => s.Id == sequenceId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes[0].Sequences[0].Shots, sh => sh.Id == shotId)
        );

        var update = Builders<Project>.Update.PullFilter(s => s.Episodes[0].Sequences[0].Shots, sh => sh.Id == shotId);

        var result = await _ProjectsCollection.UpdateOneAsync(filter, update);
    }

    // TODO : UpdateAsync
    
}