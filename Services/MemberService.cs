using CPM_backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CPM_backend.Services;

public class MembersService
{
    private readonly IMongoCollection<Member> _membersCollection;

    public MembersService(IOptions<DatabaseConfiguration> databaseConfiguration)
    {
        var mongoClient = new MongoClient(databaseConfiguration.Value.Connection);

        var mongoDatabase = mongoClient.GetDatabase(databaseConfiguration.Value.Name);

        _membersCollection = mongoDatabase.GetCollection<Member>(
            databaseConfiguration.Value.MembersCollection
        );
    }

    public async Task<List<Member>> GetAsync()
    {
        return await _membersCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Member?> GetAsync(string id)
    {
        return await _membersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Member newMember)
    {
        await _membersCollection.InsertOneAsync(newMember);
    }

    public async Task UpdateAsync(string id, Member updatedMember)
    {
        await _membersCollection.ReplaceOneAsync(x => x.Id == id, updatedMember);
    }

    public async Task RemoveAsync(string id)
    {
        await _membersCollection.DeleteOneAsync(x => x.Id == id);
    }
}