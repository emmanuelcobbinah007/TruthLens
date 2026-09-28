using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruthLens.Api.Data;
using TruthLens.Api.ML;
using TruthLens.Shared;

namespace TruthLens.Api.Controllers;

[ApiController]
[Route("api/claims")]
public class ClaimsController(ClaimClassifierService classifier, TruthLensDbContext db) : ControllerBase
{
    private static readonly Dictionary<string, string> Explanations = new()
    {
        ["Credible"] = "The wording and framing of this claim closely resemble verified, well-established statements (e.g. citing recognized health/election authorities, established facts). Always confirm against a primary source.",
        ["Uncertain"] = "This claim reads as unverified, preliminary, or opinion-flavored — similar to claims that need more evidence before being trusted. Look for corroboration from a primary or authoritative source before sharing.",
        ["LikelyMisinformation"] = "This claim's language closely resembles previously debunked misinformation patterns (sensational, unverifiable, or contradicted by scientific/official consensus). Treat with strong skepticism and check a fact-checking source.",
    };

    [HttpPost("check")]
    public async Task<ActionResult<ClaimCheckResult>> Check([FromBody] ClaimCheckRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest("Claim text is required.");
        }

        var (label, confidence, scores) = classifier.Classify(request.Text);

        SourceReputation? sourceContext = null;
        if (!string.IsNullOrWhiteSpace(request.SourceDomain))
        {
            var normalized = NormalizeDomain(request.SourceDomain);
            var entity = await db.Sources.FirstOrDefaultAsync(s => s.Domain == normalized);
            sourceContext = entity is null
                ? new SourceReputation { Domain = normalized, Name = normalized, Category = "Unknown", Notes = "This domain is not yet in our reputation directory — verify independently." }
                : new SourceReputation { Domain = entity.Domain, Name = entity.Name, Category = entity.Category, Notes = entity.Notes };
        }

        var result = new ClaimCheckResult
        {
            ClaimText = request.Text,
            Label = label,
            Confidence = confidence,
            ClassScores = scores,
            Explanation = Explanations.GetValueOrDefault(label, string.Empty),
            SourceContext = sourceContext,
        };

        return Ok(result);
    }

    private static string NormalizeDomain(string domain)
    {
        var d = domain.Trim().ToLowerInvariant();
        d = d.Replace("https://", "").Replace("http://", "");
        d = d.Split('/')[0];
        if (d.StartsWith("www."))
        {
            d = d[4..];
        }
        return d;
    }
}
