using System.Net.Http.Json;
using TruthLens.Shared;

namespace TruthLens.Client.Services;

/// <summary>Thin wrapper over the TruthLens.Api HTTP endpoints used by the Blazor pages.</summary>
public class TruthLensApiClient(HttpClient http)
{
    public async Task<ClaimCheckResult?> CheckClaimAsync(string text, string? sourceDomain)
    {
        var response = await http.PostAsJsonAsync("api/claims/check", new ClaimCheckRequest { Text = text, SourceDomain = sourceDomain });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ClaimCheckResult>();
    }

    public async Task<List<SourceReputation>> GetAllSourcesAsync()
    {
        return await http.GetFromJsonAsync<List<SourceReputation>>("api/sources") ?? [];
    }

    public async Task<SourceReputation?> LookupSourceAsync(string domain)
    {
        return await http.GetFromJsonAsync<SourceReputation>($"api/sources/{Uri.EscapeDataString(domain)}");
    }

    public async Task<FlaggedClaimSummary?> FlagClaimAsync(string claimText, string reason, string? note)
    {
        var response = await http.PostAsJsonAsync("api/flags", new FlagRequest { ClaimText = claimText, Reason = reason, Note = note });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<FlaggedClaimSummary>();
    }

    public async Task<List<FlaggedClaimSummary>> GetFlaggedClaimsAsync()
    {
        return await http.GetFromJsonAsync<List<FlaggedClaimSummary>>("api/flags") ?? [];
    }
}
