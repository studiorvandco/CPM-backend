using CPMApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;

namespace CPMApi.Services;

public class SequencesService
{
    private readonly IMongoCollection<Project> _ProjectsCollection;

    public SequencesService(IOptions<DatabaseConfiguration> DatabaseConfiguration)
    {
        var mongoClient = new MongoClient(DatabaseConfiguration.Value.Connection);

        var mongoDatabase = mongoClient.GetDatabase(DatabaseConfiguration.Value.Name);

        _ProjectsCollection = mongoDatabase.GetCollection<Project>(
            DatabaseConfiguration.Value.ProjectsCollection
        );
    }

    public async Task<List<Sequence>> GetAsync(string idProject, string idEpisode)
    {
        var project = await _ProjectsCollection.Find(p => p.Id == idProject).FirstOrDefaultAsync();

        if (project == null)
            return new List<Sequence>();

        var episode = project.Episodes.FirstOrDefault(e => e.Id == idEpisode);

        if (episode == null)
            return new List<Sequence>();

        return episode.Sequences;
    }

    public async Task<Sequence?> GetAsync(string idProject, string idEpisode, string idSequence)
    {
        var episodes = await _ProjectsCollection.Find(Builders<Project>.Filter.Eq(p => p.Id, idProject))
                                                .Project(p => p.Episodes)
                                                .ToListAsync();

        var episode = episodes.SelectMany(e => e)
                            .FirstOrDefault(e => e.Id == idEpisode);

        var sequence = episode?.Sequences.FirstOrDefault(s => s.Id == idSequence);

        return sequence;
    }

    public async Task CreateAsync(string projectId, string episodeId, Sequence sequence)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episodeId)
        );
        var update = Builders<Project>.Update.Push(e => e.Episodes[0].Sequences, sequence);

        await _ProjectsCollection.UpdateOneAsync(filter, update);
    }

    public async Task RemoveAsync(string projectId, string episodeId, string sequenceId)
    {
        var filter = Builders<Project>.Filter.And(
        Builders<Project>.Filter.Eq(p => p.Id, projectId),
        Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episodeId),
        Builders<Project>.Filter.ElemMatch(p => p.Episodes[0].Sequences, s => s.Id == sequenceId)
        );

        var update = Builders<Project>.Update.PullFilter(e => e.Episodes[0].Sequences, s => s.Id == sequenceId);

        var result = await _ProjectsCollection.UpdateOneAsync(filter, update);
    }

    // TODO: Corriger la fonction UpdateAsync()
    // Problème au niveau du filtre update

    /*public async Task UpdateAsync(string projectId, string episodeId, string sequenceId, Sequence newSequence)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episodeId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes[0].Sequences, s => s.Id == sequenceId)
        );

        var update = Builders<Project>.Update.Set("Episodes.$[elemMatch].Sequences.$", newSequence);

        var result = await _ProjectsCollection.UpdateOneAsync(filter, update);
    }*/

    /*public async Task UpdateAsync(string projectId, string episodeId, string sequenceId, Sequence newSequence)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e => e.Id == episodeId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes[0].Sequences, s => s.Id == sequenceId)
        );

        var update = Builders<Project>.Update.Set("Episodes.$[elemMatch].Sequence", newSequence);

        var options = new UpdateOptions { 
            ArrayFilters = new List<ArrayFilterDefinition<Project>> { 
                new BsonDocumentArrayFilterDefinition<Project>(
                    new BsonDocument($"episodes.id", episodeId)
                )
            } 
        };

        var result = await _ProjectsCollection.UpdateOneAsync(filter, update, options);
    }*/

}
