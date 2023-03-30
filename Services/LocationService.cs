using CPM_backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CPM_backend.Services;

public class LocationsService
{
    private readonly IMongoCollection<Location> _locationsCollection;

    public LocationsService(IOptions<DatabaseConfiguration> databaseConfiguration)
    {
        var mongoClient = new MongoClient(databaseConfiguration.Value.Connection);

        var mongoDatabase = mongoClient.GetDatabase(databaseConfiguration.Value.Name);

        _locationsCollection = mongoDatabase.GetCollection<Location>(
            databaseConfiguration.Value.LocationsCollection
        );
    }

    public async Task<List<Location>> GetAsync()
    {
        return await _locationsCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Location?> GetAsync(string id)
    {
        return await _locationsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Location newLocation)
    {
        await _locationsCollection.InsertOneAsync(newLocation);
    }

    public async Task UpdateAsync(string id, Location updatedLocation)
    {
        await _locationsCollection.ReplaceOneAsync(x => x.Id == id, updatedLocation);
    }

    public async Task RemoveAsync(string id)
    {
        await _locationsCollection.DeleteOneAsync(x => x.Id == id);
    }
}