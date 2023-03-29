using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace CPMApi.Models;

public class Location
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("Name")]
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [BsonElement("Position")]
    [JsonPropertyName("position")]
    [BsonDefaultValue("")]
    public string Position { get; set; } = "";

    /*public Location cloneLocation(){
        Location newLocation = new Location();

        newLocation.Id = this.Id;
        newLocation.Name = this.Name;
        newLocation.Position = this.Position;

        return newLocation;
    }*/
}
