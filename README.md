# TruthLens

**Category:** AI & ML — misinformation / fake-news screener.

Paste a viral claim or headline and get a credibility signal plus source context. Built for
the DCIT 318 (Programming 2) semester group project.

## What it does

- **Claim checker** — paste a claim/headline, get back a credibility label (`Credible`,
  `Uncertain`, `LikelyMisinformation`), a confidence score, per-class scores, and a short
  explanation. Optionally pass the domain the claim was seen on for source context.
- **Source reputation lookup** — look up a domain (e.g. `reuters.com`) to see how it's
  categorized (high reputation, government/health authority, mixed, satire, low
  reputation, unknown) and why.
- **Community flagging** — anyone can flag a claim as still circulating, already debunked,
  or misleading, and see the aggregated flag counts other people have submitted.

## Tech stack

| Layer          | Technology                                                  |
|----------------|--------------------------------------------------------------|
| Frontend       | Blazor WebAssembly (.NET 9)                                  |
| Backend        | ASP.NET Core Web API (.NET 9, controllers)                   |
| ML             | ML.NET (`Microsoft.ML`) — multiclass text classification     |
| Data           | EF Core + SQLite                                              |
| Shared         | A `TruthLens.Shared` class library holding request/response DTOs used by both API and client |

This is a **separate frontend/backend** project: the Blazor client is a static
WebAssembly app that talks to the API over HTTP, so it needs its own deployment
(a static host) alongside the API's deployment (an app host).

## Project layout

```
TruthLens/
  TruthLens.sln
  src/
    TruthLens.Shared/    DTOs shared by API and client
    TruthLens.Api/        ASP.NET Core Web API, ML.NET classifier, EF Core/SQLite
      Controllers/         ClaimsController, SourcesController, FlagsController
      Data/                DbContext, entities, seed data (sources + training claims)
      ML/                  ClaimClassifierService (trains + scores the ML.NET model)
      Dockerfile            for deploying the API
    TruthLens.Client/     Blazor WebAssembly frontend
      Pages/                Home (claim checker), Sources, Flags
      Services/             TruthLensApiClient (typed HttpClient wrapper)
```

## How the ML model works

`ClaimClassifierService` (in `src/TruthLens.Api/ML/ClaimClassifier.cs`) trains a
multiclass text classifier every time the API starts, using a small curated dataset of
labeled example claims in `Data/SeedData.cs` (`TrainingClaims`). The pipeline:

1. `FeaturizeText` — turns the claim text into n-gram/word-embedding-style numeric
   features (ML.NET's built-in text featurizer).
2. `SdcaMaximumEntropy` — a multiclass logistic-regression-style trainer over those
   features.
3. The trained model is kept in memory (via `PredictionEngine`) and used to score every
   `POST /api/claims/check` request.

Training on ~70 short examples takes a fraction of a second, so there's no need to ship
a separate serialized model file — it's simple to reason about and easy to demo, at the
cost of not being a production-grade classifier. See **Limitations** below.

## Running locally

Requires the .NET 9 SDK.

```bash
# Terminal 1 — API
cd src/TruthLens.Api
dotnet run
# listens on https://localhost:7035 and http://localhost:5050 (see Properties/launchSettings.json)

# Terminal 2 — Client
cd src/TruthLens.Client
dotnet run
# listens on https://localhost:7134 and http://localhost:5185, opens a browser
```

The client reads the API's base URL from `wwwroot/appsettings.json`
(`ApiBaseUrl`, defaults to `https://localhost:7035/` for local dev). The API's
`appsettings.json` has a matching `AllowedClientOrigins` CORS allowlist for the client's
local ports.

On first run the API creates `truthlens.db` (SQLite) next to the executable and seeds it
with the source-reputation directory in `Data/SeedData.cs`.

## API reference

| Method | Route                    | Body / params                              | Returns                     |
|--------|---------------------------|---------------------------------------------|------------------------------|
| POST   | `/api/claims/check`       | `{ text, sourceDomain? }`                    | `ClaimCheckResult`           |
| GET    | `/api/sources`            | —                                             | `SourceReputation[]`         |
| GET    | `/api/sources/{domain}`   | —                                             | `SourceReputation`           |
| POST   | `/api/flags`              | `{ claimText, reason, note? }`               | `FlaggedClaimSummary`        |
| GET    | `/api/flags`               | —                                             | `FlaggedClaimSummary[]`      |

## Deployment

**API** (needs a host that runs a long-lived process/container — e.g. Render, Railway,
Fly.io, Azure App Service):

1. Build the container from the **solution root** (the Dockerfile copies both
   `TruthLens.Shared` and `TruthLens.Api`):
   ```bash
   docker build -f src/TruthLens.Api/Dockerfile -t truthlens-api .
   ```
2. Set `AllowedClientOrigins` (via `appsettings.Production.json` or an environment
   variable override) to your deployed Blazor client's URL — update the placeholder in
   `src/TruthLens.Api/appsettings.Production.json`.
3. Note: the seeded SQLite file lives in the container's filesystem, which most free
   tiers do **not** persist across redeploys. That's fine for a class demo (it reseeds on
   every startup); mount a persistent volume if you need flags to survive redeploys.

**Client** (needs only a static file host — e.g. Azure Static Web Apps, Netlify, GitHub
Pages, Cloudflare Pages):

1. Update `wwwroot/appsettings.Production.json`'s `ApiBaseUrl` to your deployed API's URL.
2. Publish the static output:
   ```bash
   dotnet publish src/TruthLens.Client/TruthLens.Client.csproj -c Release -o publish
   ```
   The static site is in `publish/wwwroot`.
3. Deploy that folder to your static host of choice.

Update the two `REPLACE-WITH-...` placeholders (`TruthLens.Client/wwwroot/appsettings.Production.json`
and `TruthLens.Api/appsettings.Production.json`) before deploying — see
`docs/DEPLOYMENT_CHECKLIST.md`.

## Limitations & honest caveats (worth mentioning in the video)

- The classifier is trained on a **small, hand-written example set** (~70 claims), not a
  large real-world labeled corpus — it's a demonstration of the ML.NET pipeline, not a
  production fact-checking engine. It will generalize reasonably to claims that resemble
  its training examples (well-known health/election misinformation patterns) and much
  less reliably to novel claims.
- The source-reputation directory is a small seeded list, not a live-updated database —
  most real-world domains will come back as `Unknown`.
- Nothing here should be presented as a substitute for professional fact-checking; the
  tool gives a *signal*, not a verdict.

## Team

See `docs/PRESENTATION_GUIDE.md` for how the 12 team members split up ownership of each
part of the app for the video walkthrough.
