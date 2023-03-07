/*using CPMApi.Models;
using CPMApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPMApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShotsController : ControllerBase
{
    private readonly ShotsService _ShotsService;

    public ShotsController(ShotsService ShotsService) =>
        _ShotsService = ShotsService;

    [HttpGet, Authorize]
    public async Task<List<Shot>> Get() =>
        await _ShotsService.GetAsync();

    [HttpGet("{id:length(24)}"), Authorize]
    public async Task<ActionResult<Shot>> Get(string id)
    {
        var Shot = await _ShotsService.GetAsync(id);

        if (Shot is null)
        {
            return NotFound();
        }

        return Shot;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Post(Shot newShot)
    {
        await _ShotsService.CreateAsync(newShot);

        return CreatedAtAction(nameof(Get), new { id = newShot.Id }, newShot);
    }

    [HttpPut("{id:length(24)}"), Authorize]
    public async Task<IActionResult> Update(string id, Shot updatedShot)
    {
        var Shot = await _ShotsService.GetAsync(id);

        if (Shot is null)
        {
            return NotFound();
        } 

        Shot newShot = Shot.cloneShot();

        // WARNING : We always replace new value here
        newShot.Number = updatedShot.Number;

        if (updatedShot.Title != null)
            newShot.Title = updatedShot.Title;

        if (updatedShot.Value != null)
            newShot.Value = updatedShot.Value;

        if (updatedShot.Description != null)
            newShot.Description = updatedShot.Description;

        if (updatedShot.Completed != false)
            newShot.Completed = updatedShot.Completed;

        await _ShotsService.UpdateAsync(id, newShot);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}"), Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var Shot = await _ShotsService.GetAsync(id);

        if (Shot is null)
        {
            return NotFound();
        }

        await _ShotsService.RemoveAsync(id);

        return NoContent();
    }
}
*/