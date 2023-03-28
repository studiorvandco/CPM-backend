using CPMApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CPMApi.Services;

public class UsersService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UsersService(IOptions<DatabaseConfiguration> DatabaseConfiguration)
    {
        var mongoClient = new MongoClient(DatabaseConfiguration.Value.Connection);

        var mongoDatabase = mongoClient.GetDatabase(DatabaseConfiguration.Value.Name);

        _usersCollection = mongoDatabase.GetCollection<User>(
            DatabaseConfiguration.Value.UsersCollection
        );
    }

    public async Task<List<User>> GetAsync() =>
        await _usersCollection.Find(_ => true).ToListAsync();

    public async Task<User?> GetAsync(string id) =>
        await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(User newUser) =>
        await _usersCollection.InsertOneAsync(newUser);

    public async Task UpdateAsync(string id, UserUpdateDTO updatedUser)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id, id);
        var user = await _usersCollection.Find(filter).FirstOrDefaultAsync();

        if (user == null) return;

        if (updatedUser.Password != null)
            user.Password = updatedUser.Password;

        var update = Builders<User>.Update
            .Set(u => u.Username, updatedUser.Username ?? user.Username)
            .Set(u => u.Hash, user.Hash);

        await _usersCollection.UpdateOneAsync(filter, update);
    }

    public async Task RemoveAsync(string id) =>
        await _usersCollection.DeleteOneAsync(x => x.Id == id);
}
