using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CPM_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using BC = BCrypt.Net.BCrypt;

namespace CPM_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public IActionResult Login(
        Login login,
        IOptions<DatabaseConfiguration> databaseConfiguration,
        IOptions<LoginConfiguration> loginConfiguration
    )
    {
        try
        {
            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
                return BadRequest("Username or Password not specified");
            if (IsAuthorized(login, databaseConfiguration))
            {
                var secretKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(loginConfiguration.Value.Key)
                );
                var signinCredentials = new SigningCredentials(
                    secretKey,
                    SecurityAlgorithms.HmacSha256
                );
                var jwtSecurityToken = new JwtSecurityToken(
                    loginConfiguration.Value.Issuer,
                    loginConfiguration.Value.Audience,
                    new List<Claim>(),
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

    private static bool IsAuthorized(Login login, IOptions<DatabaseConfiguration> databaseConfiguration)
    {
        var mongoClient = new MongoClient(databaseConfiguration.Value.Connection);
        var mongoDatabase = mongoClient.GetDatabase(databaseConfiguration.Value.Name);
        var usersCollection = mongoDatabase.GetCollection<User>(
            databaseConfiguration.Value.UsersCollection
        );
        var users = usersCollection.Find(new BsonDocument()).ToList();

        foreach (var user in users)
            if (user.Username.Equals(login.Username) && BC.Verify(login.Password, user.Hash))
                return true;

        return false;
    }
}