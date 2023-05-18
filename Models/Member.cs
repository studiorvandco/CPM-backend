using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CPM_backend.Models;

public class Member
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("FirstName")]
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = null!;

    [BsonElement("LastName")]
    [JsonPropertyName("last_name")]
    [BsonDefaultValue("")]
    public string LastName { get; set; } = "";

    [BsonElement("Phone")]
    [JsonPropertyName("phone")]
    [BsonDefaultValue("")]
    public string Phone { get; set; } = "";

    /*public Member cloneMember(){
        Member newMember = new Member();

        newMember.Id = this.Id;
        newMember.FirstName = this.FirstName;
        newMember.LastName = this.LastName;
        newMember.PhoneNumber = this.PhoneNumber;

        return newMember;
    }*/
}
