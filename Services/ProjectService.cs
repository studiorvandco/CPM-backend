using CPM_backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CPM_backend.Services;

public class ProjectsService
{
    private readonly IMongoCollection<Project> _projectsCollection;

    public ProjectsService(IOptions<DatabaseConfiguration> databaseConfiguration)
    {
        var mongoClient = new MongoClient(databaseConfiguration.Value.Connection);

        var mongoDatabase = mongoClient.GetDatabase(databaseConfiguration.Value.Name);

        _projectsCollection = mongoDatabase.GetCollection<Project>(
            databaseConfiguration.Value.ProjectsCollection
        );
    }

    public async Task<List<Project>> GetAsync()
    {
        return await _projectsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Project?> GetAsync(string id)
    {
        return await _projectsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Project newProject)
    {
        await _projectsCollection.InsertOneAsync(newProject);
    }

    public async Task UpdateAsync(string id, ProjectUpdateDTO updatedProject)
    {
        var filter = Builders<Project>.Filter.Eq(p => p.Id, id);
        var project = await _projectsCollection.Find(filter).FirstOrDefaultAsync();

        if (project == null)
            return;

        var update = Builders<Project>.Update
            .Set(p => p.Title, updatedProject.Title ?? project.Title)
            .Set(p => p.Description, updatedProject.Description ?? project.Description)
            .Set(p => p.StartDate, updatedProject.BeginDate ?? project.StartDate)
            .Set(p => p.EndDate, updatedProject.EndDate ?? project.EndDate);

        await _projectsCollection.UpdateOneAsync(filter, update);
    }

    public async Task RemoveAsync(string id)
    {
        await _projectsCollection.DeleteOneAsync(x => x.Id == id);
    }
}