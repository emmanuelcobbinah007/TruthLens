namespace TruthLens.Shared;

/// <summary>Request body for POST /api/claims/check.</summary>
public class ClaimCheckRequest
{
    public string Text { get; set; } = string.Empty;

    /// <summary>Optional domain the claim was seen on, e.g. "example-news.com".</summary>
    public string? SourceDomain { get; set; }
}

/// <summary>Result returned by the claim checker.</summary>
public class ClaimCheckResult
{
    public string ClaimText { get; set; } = string.Empty;

    /// <summary>One of: Credible, Uncertain, LikelyMisinformation.</summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>Model confidence for the predicted label, 0-1.</summary>
    public float Confidence { get; set; }

    /// <summary>Per-class scores, for transparency in the UI.</summary>
    public Dictionary<string, float> ClassScores { get; set; } = new();

    /// <summary>Short human-readable rationale shown alongside the signal.</summary>
    public string Explanation { get; set; } = string.Empty;

    /// <summary>Reputation context of the cited source, if a domain was supplied.</summary>
    public SourceReputation? SourceContext { get; set; }
}

/// <summary>A known publisher/domain and its reputation category, used for source context.</summary>
public class SourceReputation
{
    public string Domain { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    /// <summary>One of: HighReputation, GovernmentOrHealthAuthority, Mixed, Satire, LowReputation, Unknown.</summary>
    public string Category { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

/// <summary>Request body for POST /api/flags — a community member flagging a claim.</summary>
public class FlagRequest
{
    public string ClaimText { get; set; } = string.Empty;

    /// <summary>One of: StillCirculating, AlreadyDebunked, MisleadingContext, Other.</summary>
    public string Reason { get; set; } = string.Empty;

    public string? Note { get; set; }
}

/// <summary>A claim with its aggregated community flag count, for the flags feed.</summary>
public class FlaggedClaimSummary
{
    public int Id { get; set; }
    public string ClaimText { get; set; } = string.Empty;
    public int FlagCount { get; set; }
    public DateTime LastFlaggedUtc { get; set; }
    public List<string> Reasons { get; set; } = new();
}
