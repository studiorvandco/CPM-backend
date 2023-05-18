namespace CPM_backend.Models;

public class LoginConfiguration
{
    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public string Key { get; set; } = null!;

    public int Expire { get; set; } = 0;
}
