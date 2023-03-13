using CPMApi.Models;
using CPMApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPMApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SequencesController : ControllerBase
{
    private readonly SequencesService _SequencesService;

    public SequencesController(SequencesService SequencesService) =>
        _SequencesService = SequencesService;

    [HttpGet("{idProject:length(24)}/{idEpisode:length(24)}"), Authorize]
    public async Task<List<Sequence>> Get(string idProject, string idEpisode) =>
        await _SequencesService.GetAsync(idProject, idEpisode);

    [HttpGet("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}"), Authorize]
    public async Task<ActionResult<Sequence>> Get(string idProject, string idEpisode, string idSequence)
    {
        var Sequence = await _SequencesService.GetAsync(idProject, idEpisode, idSequence);

        if (Sequence is null)
        {
            return NotFound();
        }

        return Sequence;
    }

    [HttpPost("{idProject:length(24)}/{idEpisode:length(24)}"), Authorize]
    public async Task<IActionResult> Post(string idProject, string idEpisode, Sequence sequence)
    {
        if (sequence.Id == null)
            sequence.setId();

        await _SequencesService.CreateAsync(idProject, idEpisode, sequence);

        return CreatedAtAction(nameof(Get), new { idProject = idProject, idEpisode = sequence.Id }, sequence);
    }

    //A décommenter quand le service Update sera fonctionnel

    /*[HttpPut("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}"), Authorize]
    public async Task<IActionResult> Update(string idProject, string idEpisode, string idSequence, Sequence uptadeEpisode)
    {
        var sequence = await _SequencesService.GetAsync(idProject, idEpisode, idSequence);

        if (sequence is null)
        {
            return NotFound();
        } 

        uptadeEpisode.Id = sequence.Id;

        await _SequencesService.UpdateAsync(idProject, idEpisode, idSequence, uptadeEpisode);

        return NoContent();
    }*/

    [HttpDelete("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}"), Authorize]
    public async Task<IActionResult> Delete(string idProject, string idEpisode, string idSequence)
    {
        var sequence = await _SequencesService.GetAsync(idProject, idEpisode, idSequence);

        if (sequence is null)
        {
            return NotFound();
        }

        await _SequencesService.RemoveAsync(idProject, idEpisode, idSequence);

        return NoContent();
    }
}