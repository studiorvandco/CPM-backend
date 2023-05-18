using CPM_backend.Models;
using CPM_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CPM_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SequencesController : ControllerBase
{
    private readonly SequencesService _sequencesService;

    public SequencesController(SequencesService sequencesService)
    {
        _sequencesService = sequencesService;
    }

    [HttpGet("{idProject:length(24)}/{idEpisode:length(24)}")]
    [Authorize]
    public async Task<List<Sequence>> Get(string idProject, string idEpisode)
    {
        return await _sequencesService.GetAsync(idProject, idEpisode);
    }

    [HttpGet("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}")]
    [Authorize]
    public async Task<ActionResult<Sequence>> Get(
        string idProject,
        string idEpisode,
        string idSequence
    )
    {
        var sequence = await _sequencesService.GetAsync(idProject, idEpisode, idSequence);

        if (sequence is null)
            return NotFound();

        return sequence;
    }

    [HttpPost("{idProject:length(24)}/{idEpisode:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Post(string idProject, string idEpisode, Sequence sequence)
    {
        await _sequencesService.CreateAsync(idProject, idEpisode, sequence);

        return CreatedAtAction(
            nameof(Get),
            new
            {
                idProject,
                idEpisode,
                idSequence = sequence.Id
            },
            sequence
        );
    }

    [HttpPut("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Update(
        string idProject,
        string idEpisode,
        string idSequence,
        SequenceUpdateDTO updatedSequence
    )
    {
        var sequence = await _sequencesService.GetAsync(idProject, idEpisode, idSequence);

        if (sequence is null)
            return NotFound();

        await _sequencesService.UpdateAsync(idProject, idEpisode, idSequence, updatedSequence);

        return NoContent();
    }

    [HttpDelete("{idProject:length(24)}/{idEpisode:length(24)}/{idSequence:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Delete(string idProject, string idEpisode, string idSequence)
    {
        var sequence = await _sequencesService.GetAsync(idProject, idEpisode, idSequence);

        if (sequence is null)
            return NotFound();

        await _sequencesService.RemoveAsync(idProject, idEpisode, sequence);

        return NoContent();
    }
}