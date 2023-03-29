using CPMApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;

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

        var episode = (await _ProjectsCollection.Find(filter).FirstOrDefaultAsync())
                        ?.Episodes.FirstOrDefault(e => e.Id == episodeId);
        if (episode == null) return;

        if (episode.Sequences.Count == 0) {
            sequence.Number = 1;
        } else {
            sequence.Number = episode.Sequences.Max(e => e.Number) + 1;
        }

        var update = Builders<Project>.Update.Push(e => e.Episodes.FirstMatchingElement().Sequences, sequence);

        await _ProjectsCollection.UpdateOneAsync(filter, update);
    }

    public async Task RemoveAsync(string projectId, string episodeId, Sequence sequence)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e =>
                e.Id == episodeId && e.Sequences.Any(s => s.Id == sequence.Id)
            )
        );

        var update = Builders<Project>.Update
            .PullFilter(p =>
                p.Episodes.FirstMatchingElement().Sequences, s => s.Id == sequence.Id)
            .Inc(p => p.Episodes.FirstMatchingElement().ShotsTotal, -sequence.ShotsTotal)
            .Inc(p => p.Episodes.FirstMatchingElement().ShotsCompleted, -sequence.ShotsCompleted)
            .Inc(p => p.ShotsTotal, -sequence.ShotsTotal)
            .Inc(p => p.ShotsCompleted, -sequence.ShotsCompleted);


        var result = await _ProjectsCollection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateAsync(string projectId, string episodeId, string sequenceId, SequenceUpdateDTO updatedSequence)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e =>
                e.Id == episodeId && e.Sequences.Any(s => s.Id == sequenceId)
            )
        );
        
        var sequence = (await _ProjectsCollection.Find(filter).FirstOrDefaultAsync())
                            ?.Episodes.FirstOrDefault(e => e.Id == episodeId)
                            ?.Sequences.FirstOrDefault(s => s.Id == sequenceId);
        if (sequence == null) return;

        var update = Builders<Project>.Update
            .Set(
                p => p.Episodes.AllMatchingElements("e")
                      .Sequences.AllMatchingElements("s").Title, 
                updatedSequence.Title ?? sequence.Title)
            .Set(
                p => p.Episodes.AllMatchingElements("e")
                      .Sequences.AllMatchingElements("s").Description, 
                updatedSequence.Description ?? sequence.Description)
            .Set(
                p => p.Episodes.AllMatchingElements("e")
                      .Sequences.AllMatchingElements("s").BeginDate, 
                updatedSequence.BeginDate ?? sequence.BeginDate)
            .Set(
                p => p.Episodes.AllMatchingElements("e")
                      .Sequences.AllMatchingElements("s").EndDate, 
                updatedSequence.EndDate ?? sequence.EndDate)
            .Set(
                p => p.Episodes.AllMatchingElements("e")
                      .Sequences.AllMatchingElements("s").Number, 
                updatedSequence.Number == 0 ? sequence.Number : updatedSequence.Number);
                
        var options = new UpdateOptions {
            ArrayFilters = new List<ArrayFilterDefinition> {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("e._id", new ObjectId(episodeId))),
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("s._id", new ObjectId(sequenceId)))
            }
        };
        
        await _ProjectsCollection.UpdateOneAsync(filter, update, options);
    }

}
