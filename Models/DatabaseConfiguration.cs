namespace CPMApi.Models;

public class DatabaseConfiguration
{
    public string Connection { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string UsersCollection { get; set; } = null!;

    public string LocationsCollection { get; set; } = null!;

    public string ProjectsCollection { get; set; } = null!;

    public string MembersCollection { get; set; } = null!;
}
