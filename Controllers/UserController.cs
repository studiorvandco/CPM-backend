using CPMApi.Models;
using CPMApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPMApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService) =>
        _usersService = usersService;

    [HttpGet, Authorize]
    public async Task<List<UserOutDTO>> Get() =>
        (await _usersService.GetAsync()).Select(u => u.ToOutDTO()).ToList();

    [HttpGet("{id:length(24)}"), Authorize]
    public async Task<ActionResult<UserOutDTO>> Get(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user.ToOutDTO();
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Post(UserInDTO newUser)
    {
        var user = newUser.ToUser();
        if (user == null)
            return BadRequest();
        await _usersService.CreateAsync(user);

        return CreatedAtAction(nameof(Get), new { id = user.Id }, user.ToOutDTO());
    }

    [HttpPut("{id:length(24)}"), Authorize]
    public async Task<IActionResult> Update(string id, UserUpdateDTO updatedUser)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }
        
        updatedUser.Id = user.Id;

        await _usersService.UpdateAsync(id, updatedUser);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}"), Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _usersService.RemoveAsync(id);

        return NoContent();
    }
}
