using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Location
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("Name")]
    [JsonPropertyName("Name")]
    public string? Name { get; set; } = null!;

    [BsonElement("Link")]
    [JsonPropertyName("Link")]
    public string? Link { get; set; }

    public Location cloneLocation(){
        Location newLocation = new Location();

        newLocation.Id = this.Id;
        newLocation.Name = this.Name;
        newLocation.Link = this.Link;

        return newLocation;
    }
}
