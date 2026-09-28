using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruthLens.Api.Data;
using TruthLens.Shared;

namespace TruthLens.Api.Controllers;

[ApiController]
[Route("api/sources")]
public class SourcesController(TruthLensDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SourceReputation>>> GetAll()
    {
        var sources = await db.Sources
            .OrderBy(s => s.Name)
            .Select(s => new SourceReputation { Domain = s.Domain, Name = s.Name, Category = s.Category, Notes = s.Notes })
            .ToListAsync();

        return Ok(sources);
    }

    [HttpGet("{domain}")]
    public async Task<ActionResult<SourceReputation>> GetByDomain(string domain)
    {
        var normalized = domain.Trim().ToLowerInvariant();
        var entity = await db.Sources.FirstOrDefaultAsync(s => s.Domain == normalized);

        if (entity is null)
        {
            return Ok(new SourceReputation
            {
                Domain = normalized,
                Name = normalized,
                Category = "Unknown",
                Notes = "This domain is not yet in our reputation directory — verify independently.",
            });
        }

        return Ok(new SourceReputation { Domain = entity.Domain, Name = entity.Name, Category = entity.Category, Notes = entity.Notes });
    }
}
