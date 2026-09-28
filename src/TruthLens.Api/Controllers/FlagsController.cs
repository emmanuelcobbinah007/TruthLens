using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruthLens.Api.Data;
using TruthLens.Shared;

namespace TruthLens.Api.Controllers;

[ApiController]
[Route("api/flags")]
public class FlagsController(TruthLensDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<FlaggedClaimSummary>> Create([FromBody] FlagRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ClaimText) || string.IsNullOrWhiteSpace(request.Reason))
        {
            return BadRequest("ClaimText and Reason are required.");
        }

        var claimText = request.ClaimText.Trim();

        var claim = await db.FlaggedClaims
            .Include(c => c.Flags)
            .FirstOrDefaultAsync(c => c.ClaimText == claimText);

        if (claim is null)
        {
            claim = new FlaggedClaimEntity { ClaimText = claimText };
            db.FlaggedClaims.Add(claim);
        }

        claim.Flags.Add(new FlagEntity { Reason = request.Reason, Note = request.Note });
        await db.SaveChangesAsync();

        return Ok(ToSummary(claim));
    }

    [HttpGet]
    public async Task<ActionResult<List<FlaggedClaimSummary>>> GetAll()
    {
        var claims = await db.FlaggedClaims
            .Include(c => c.Flags)
            .OrderByDescending(c => c.Flags.Max(f => (DateTime?)f.CreatedUtc) ?? c.CreatedUtc)
            .ToListAsync();

        return Ok(claims.Select(ToSummary).ToList());
    }

    private static FlaggedClaimSummary ToSummary(FlaggedClaimEntity claim) => new()
    {
        Id = claim.Id,
        ClaimText = claim.ClaimText,
        FlagCount = claim.Flags.Count,
        LastFlaggedUtc = claim.Flags.Count > 0 ? claim.Flags.Max(f => f.CreatedUtc) : claim.CreatedUtc,
        Reasons = claim.Flags.Select(f => f.Reason).Distinct().ToList(),
    };
}
