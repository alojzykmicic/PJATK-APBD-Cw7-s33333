using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTOs;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PCsController : ControllerBase
{
    private readonly IPCService _pcService;
    
    public PCsController(IPCService pcService)
    {
        _pcService = pcService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PcDto>>> GetAllPCs()
    {
        var result = await _pcService.GetAllPCsAsync();
        return Ok(result);
    }
    
    [HttpGet("{id}/components")]
    public async Task<ActionResult<IEnumerable<PcComponentDetailDto>>> GetPCComponents(int id)
    {
        var components = await _pcService.GetPCComponentsAsync(id);
        
        if (components == null)
        {
            return NotFound(new { message = $"Computer with ID = {id} not found." });
        }
        return Ok(components);
    }
    
    [HttpPost]
    public async Task<ActionResult<PcDto>> CreatePC([FromBody] PCRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdPc = await _pcService.CreatePCAsync(request);
        
        return CreatedAtAction(nameof(GetAllPCs), new { id = createdPc.Id }, createdPc);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePC(int id, [FromBody] PCRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updated = await _pcService.UpdatePCAsync(id, request);
            
        if (!updated)
        {
            return NotFound(new { message = $"Computer with ID = {id} not found." });
        }

        return Ok(new { message = "Computer details updated successfully." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePC(int id)
    {
        var deleted = await _pcService.DeletePCAsync(id);

        if (!deleted)
        {
            return NotFound(new { message = $"Computer with ID = {id} not found." });
        }

        return NoContent();
    }
}