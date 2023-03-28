using CPMApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;

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
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e =>
                e.Id == episodeId && e.Sequences.Any(s => s.Id == sequenceId)
            )
        );

        if (shot.Number <= 0) {
            var sequence = (await _ProjectsCollection.Find(filter).FirstOrDefaultAsync())
                            ?.Episodes.FirstOrDefault(e => e.Id == episodeId)
                            ?.Sequences.FirstOrDefault(s => s.Id == sequenceId);
            if (sequence == null) return;

            if (sequence.Shots.Count == 0) {
                shot.Number = 1;
            } else {
                shot.Number = sequence.Shots.Max(e => e.Number) + 1;
            }
        }

        var update = Builders<Project>.Update.Push(
            p => p.Episodes.AllMatchingElements("e")
                  .Sequences.AllMatchingElements("s").Shots, 
            shot
        );

        var options = new UpdateOptions {
            ArrayFilters = new List<ArrayFilterDefinition> {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("e._id", new ObjectId(episodeId))),
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("s._id", new ObjectId(sequenceId)))
            }
        };

        await _ProjectsCollection.UpdateOneAsync(filter, update, options);
    }

    public async Task RemoveAsync(string projectId, string episodeId, string sequenceId, string shotId)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e =>
                e.Id == episodeId && e.Sequences.Any(s =>
                    s.Id == sequenceId && s.Shots.Any(h => 
                        h.Id == shotId
                    )
                )
            )
        );

        var update = Builders<Project>.Update.PullFilter(s =>
            s.Episodes.AllMatchingElements("e")
                .Sequences.AllMatchingElements("s").Shots,
            h => h.Id == shotId
        );

        var options = new UpdateOptions {
            ArrayFilters = new List<ArrayFilterDefinition> {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("e._id", new ObjectId(episodeId))),
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("s._id", new ObjectId(sequenceId)))
            }
        };

        var result = await _ProjectsCollection.UpdateOneAsync(filter, update, options);
    }

    public async Task UpdateAsync(string projectId, string episodeId, string sequenceId, string shotId, ShotUpdateDTO updatedShot)
    {
        var filter = Builders<Project>.Filter.And(
            Builders<Project>.Filter.Eq(p => p.Id, projectId),
            Builders<Project>.Filter.ElemMatch(p => p.Episodes, e =>
                e.Id == episodeId && e.Sequences.Any(s =>
                    s.Id == sequenceId && s.Shots.Any(h => 
                        h.Id == shotId
                    )
                )
            )
        );

        var shot = (await _ProjectsCollection.Find(filter).FirstOrDefaultAsync())
                        ?.Episodes.FirstOrDefault(e => e.Id == episodeId)
                        ?.Sequences.FirstOrDefault(s => s.Id == sequenceId)
                        ?.Shots.FirstOrDefault(h => h.Id == shotId);
        if (shot == null) return;

        var update = Builders<Project>.Update
            .Set(
                p => p.Episodes.AllMatchingElements("e")
                      .Sequences.AllMatchingElements("s")
                      .Shots.AllMatchingElements("h").Title, 
                updatedShot.Title ?? shot.Title)
            .Set(
                p => p.Episodes.AllMatchingElements("e")
                      .Sequences.AllMatchingElements("s")
                      .Shots.AllMatchingElements("h").Description, 
                updatedShot.Description ?? shot.Description)
            .Set(
                p => p.Episodes.AllMatchingElements("e")
                      .Sequences.AllMatchingElements("s")
                      .Shots.AllMatchingElements("h").Value, 
                updatedShot.Value ?? shot.Value)
            .Set(
                p => p.Episodes.AllMatchingElements("e")
                      .Sequences.AllMatchingElements("s")
                      .Shots.AllMatchingElements("h").Number, 
                updatedShot.Number == 0 ? shot.Number : updatedShot.Number);
                
        var options = new UpdateOptions {
            ArrayFilters = new List<ArrayFilterDefinition> {
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("e._id", new ObjectId(episodeId))),
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("s._id", new ObjectId(sequenceId))),
                new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("h._id", new ObjectId(shotId)))
            }
        };
        
        await _ProjectsCollection.UpdateOneAsync(filter, update, options);
    }
    
}