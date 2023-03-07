/*using CPMApi.Models;
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

    public async Task<List<Shot>> GetShotsAsync(string projectId, string episodeId, string sequenceId)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episodeId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes[-1].Sequences, s => s.Id == sequenceId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes[-1].Sequences[-1].Shots, sh => true)
        );

        var projection = Builders<Project>.Projection
            .Include(p => p.Episodes[-1].Sequences[-1].Shots);

        var result = await _ProjectsCollection.Find(filter).Project(projection).FirstOrDefaultAsync();

        if (result == null || result.Episodes.Count == 0 || result.Episodes[0].Sequences.Count == 0)
        {
            return new List<Shot>();
        }

        return result.Episodes[0].Sequences[0].Shots;
    }

public async Task<List<Shot>> GetAsync(string projectId, string episodeId, string sequenceId)
{
    var filterBuilder = Builders<Project>.Filter;
    var filter = filterBuilder.And(
        filterBuilder.Eq(p => p.Id, projectId),
        filterBuilder.ElemMatch(p => p.Episodes, e => e.Id == episodeId),
        filterBuilder.ElemMatch(p => p.Episodes[-1].Sequences, s => s.Id == sequenceId)
    );

    var projectionBuilder = Builders<Project>.Projection;
    var projection = projectionBuilder.Include("Episodes.Sequences.Shots");

    var result = await _ProjectsCollection.Find(filter)
                                .Project(projection)
                                .FirstOrDefaultAsync();

    if (result == null)
    {
        return new List<Shot>();
    }

    var episode = result.Episodes.FirstOrDefault(e => e.Id == episodeId);
    if (episode == null)
    {
        return new List<Shot>();
    }

    var sequence = episode.Sequences.FirstOrDefault(s => s.Id == sequenceId);
    if (sequence == null)
    {
        return new List<Shot>();
    }

    return sequence.Shots ?? new List<Shot>();
}



    //public async Task<List<Shot>> GetAsync() =>
      //  await _ProjectsCollection.Find(_ => true).ToListAsync();

    //public async Task<Shot?> GetAsync(string id) =>
      //  await _ShotsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    //public async Task CreateAsync(Shot newShot) =>
      //  await _ShotsCollection.InsertOneAsync(newShot);

    //public async Task UpdateAsync(string id, Shot updatedShot) =>
      //  await _ShotsCollection.ReplaceOneAsync(x => x.Id == id, updatedShot);

    //public async Task RemoveAsync(string id) =>
      //  await _ShotsCollection.DeleteOneAsync(x => x.Id == id);
}
*/