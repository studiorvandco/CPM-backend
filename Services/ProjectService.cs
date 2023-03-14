using CPMApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CPMApi.Services;

public class ProjectsService
{
    private readonly IMongoCollection<Project> _ProjectsCollection;

    public ProjectsService(IOptions<DatabaseConfiguration> DatabaseConfiguration)
    {
        var mongoClient = new MongoClient(DatabaseConfiguration.Value.Connection);

        var mongoDatabase = mongoClient.GetDatabase(DatabaseConfiguration.Value.Name);

        _ProjectsCollection = mongoDatabase.GetCollection<Project>(
            DatabaseConfiguration.Value.ProjectsCollection
        );
    }

    public async Task<List<Project>> GetAsync() =>
        await _ProjectsCollection.Find(_ => true).ToListAsync();

    public async Task<Project?> GetAsync(string id) =>
        await _ProjectsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Project newProject) =>
        await _ProjectsCollection.InsertOneAsync(newProject.WithDefaults());

    public async Task UpdateAsync(string id, Project updatedProject) {
        var filter = Builders<Project>.Filter.Eq(p => p.Id, id);
        var project = await _ProjectsCollection.Find(filter).FirstOrDefaultAsync();

        if (project == null) {
            await CreateAsync(updatedProject.WithDefaults());
        } else {
            var update = Builders<Project>.Update
                .Set(p => p.Title, updatedProject.Title ?? project.Title)
                .Set(p => p.Description, updatedProject.Description ?? project.Description)
                .Set(p => p.BeginDate, updatedProject.BeginDate ?? project.BeginDate)
                .Set(p => p.EndDate, updatedProject.EndDate ?? project.EndDate);
            
            await _ProjectsCollection.UpdateOneAsync(filter, update);
        }
    }

    public async Task RemoveAsync(string id) =>
        await _ProjectsCollection.DeleteOneAsync(x => x.Id == id);
}
