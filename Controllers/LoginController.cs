using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using CPMApi.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using BC = BCrypt.Net.BCrypt;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost, Route("login")]
    public IActionResult Login(
        Login login,
        IOptions<DatabaseConfiguration> databaseConfiguration,
        IOptions<LoginConfiguration> loginConfiguration
    )
    {
        try
        {
            if (String.IsNullOrEmpty(login.Username) || String.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Username or Password not specified");
            }
            if (isAuthorized(login, databaseConfiguration))
            {
                var secretKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(loginConfiguration.Value.Key)
                );
                var signinCredentials = new SigningCredentials(
                    secretKey,
                    SecurityAlgorithms.HmacSha256
                );
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: loginConfiguration.Value.Issuer,
                    audience: loginConfiguration.Value.Audience,
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(loginConfiguration.Value.Expire),
                    signingCredentials: signinCredentials
                );
                return Ok(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("An error occurred while generating the token");
        }

        return Unauthorized();
    }

    private bool isAuthorized(Login login, IOptions<DatabaseConfiguration> databaseConfiguration)
    {
        var mongoClient = new MongoClient(databaseConfiguration.Value.Connection);
        var mongoDatabase = mongoClient.GetDatabase(databaseConfiguration.Value.Name);
        var usersCollection = mongoDatabase.GetCollection<User>(
            databaseConfiguration.Value.UsersCollection
        );
        var users = usersCollection.Find(new BsonDocument()).ToList();

        foreach (User user in users)
        {
            if (user.Username.Equals(login.Username) && BC.Verify(login.Password, user.Hash))
            {
                return true;
            }
        }

        return false;
    }
}
