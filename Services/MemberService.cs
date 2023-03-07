using CPMApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CPMApi.Services;

public class MembersService
{
    private readonly IMongoCollection<Member> _membersCollection;

    public MembersService(IOptions<DatabaseConfiguration> DatabaseConfiguration)
    {
        var mongoClient = new MongoClient(DatabaseConfiguration.Value.Connection);

        var mongoDatabase = mongoClient.GetDatabase(DatabaseConfiguration.Value.Name);

        _membersCollection = mongoDatabase.GetCollection<Member>(
            DatabaseConfiguration.Value.MembersCollection
        );
    }

    public async Task<List<Member>> GetAsync() =>
        await _membersCollection.Find(_ => true).ToListAsync();

    public async Task<Member?> GetAsync(string id) =>
        await _membersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Member newMember) =>
        await _membersCollection.InsertOneAsync(newMember);

    public async Task UpdateAsync(string id, Member updatedMember) =>
        await _membersCollection.ReplaceOneAsync(x => x.Id == id, updatedMember);

    public async Task RemoveAsync(string id) =>
        await _membersCollection.DeleteOneAsync(x => x.Id == id);
}