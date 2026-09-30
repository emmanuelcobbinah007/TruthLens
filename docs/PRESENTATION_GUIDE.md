# TruthLens — Group Video Presentation Guide

The rubric requires **every member's face on camera**, each explaining **their own
contribution**. Since the codebase was built in one pass rather than 12 separate PRs,
use this guide to split ownership of the finished app into 12 real, distinct slices —
pick a slice, read the pointers, **actually open that code and run that part yourself**
before the video so you can speak to it naturally instead of reading a script.

**Fill in your name next to a role below** (edit this file and commit it) so everyone
knows who's covering what before you record.

Suggested video order: 1 → 12. Aim for ~45–90 seconds per person (a 12-person video
comfortably runs 12–18 minutes total).

---

### 1. Problem statement & project overview — Irene
What TruthLens is and why it matters: health and election misinformation as a global
problem, the three features (claim checker, source reputation, community flagging).
- Talking points: who this is for, what "credibility signal" means (a signal, not a
  verdict), the AI & ML category framing.
- Reference: `README.md` (top section), the original project brief.

### 2. System architecture — Philipa Yeboah
How the three projects fit together: Blazor WebAssembly client ↔ ASP.NET Core Web API ↔
ML.NET + SQLite.
- Talking points: why frontend/backend are separate (two independent deployments), what
  `TruthLens.Shared` is for, request flow for a claim check from browser to model and back.
- Reference: `README.md` → "Tech stack" / "Project layout", `TruthLens.sln`.

### 3. Claim checker — frontend — Purity Kyei
Walk through the claim-checker UI: pasting a claim, optional source domain, reading the
credibility badge, confidence bar, and class-score breakdown.
- Talking points: the three labels (Credible/Uncertain/LikelyMisinformation) and their
  color coding, how the UI calls the API.
- Reference: `src/TruthLens.Client/Pages/Home.razor`.

### 4. Claim checker — backend endpoint — Frederick Kankam
Walk through what happens server-side when `POST /api/claims/check` is hit: validating
input, calling the classifier, attaching source context, building the explanation text.
- Talking points: why the explanation is templated per-label, how source context gets
  attached when a domain is supplied.
- Reference: `src/TruthLens.Api/Controllers/ClaimsController.cs`.

### 5. The ML.NET model — Ebenezer
Explain the actual machine learning: text featurization → SDCA maximum entropy
multiclass trainer → prediction engine.
- Talking points: why it retrains at startup instead of loading a saved model file, what
  "featurizing text" means at a high level, what the per-class Score array represents.
- Reference: `src/TruthLens.Api/ML/ClaimClassifier.cs`.

### 6. Training dataset & its limits — Johnson Kuzagbe
Explain how the labeled example claims were curated (health and election misinformation
patterns, credible official-guidance statements, uncertain/ambiguous claims), and be
upfront about the dataset being small and hand-written rather than a large real-world
corpus.
- Talking points: the three label categories with 1-2 concrete examples each, why this
  matters for grading honesty (a good project acknowledges its own limitations).
- Reference: `src/TruthLens.Api/Data/SeedData.cs` → `TrainingClaims`, `README.md` →
  "Limitations & honest caveats".

### 7. Source reputation lookup — frontend — Jessica Obeng
Walk through the source lookup page: searching a domain, reading the category badge, and
the full directory table.
- Talking points: what each category means (HighReputation, GovernmentOrHealthAuthority,
  Mixed, Satire, LowReputation, Unknown).
- Reference: `src/TruthLens.Client/Pages/Sources.razor`.

### 8. Source reputation — backend & seed data — Moses Ampadu
Explain how the reputation directory is stored and served, and how an unknown domain is
handled gracefully instead of erroring.
- Talking points: why a couple of `.example` domains were used for the Mixed/LowReputation
  categories instead of naming real contested outlets — a deliberate, defensible choice.
- Reference: `src/TruthLens.Api/Controllers/SourcesController.cs`,
  `src/TruthLens.Api/Data/SeedData.cs` → `Sources`.

### 9. Community flagging — frontend — Emmanuel Cobbinah
Walk through flagging a claim from the checker result card (reason dropdown, optional
note) and the community flags feed page.
- Talking points: the four flag reasons, how flag counts aggregate per claim text.
- Reference: `src/TruthLens.Client/Pages/Home.razor` (flag form section),
  `src/TruthLens.Client/Pages/Flags.razor`.

### 10. Community flagging — backend & database — Victor Barnieh
Explain the data model: a flagged claim can have many flags, how a repeat flag on the
same claim text is aggregated rather than duplicated as a new claim row.
- Talking points: `FlaggedClaimEntity`/`FlagEntity` relationship, the summary shape
  returned to the client.
- Reference: `src/TruthLens.Api/Controllers/FlagsController.cs`,
  `src/TruthLens.Api/Data/TruthLensDbContext.cs`.

### 11. Data & persistence — Kobby24
Zoom out on the database layer as a whole: EF Core + SQLite, how the schema is created
(`EnsureCreated`) and seeded on first run, why SQLite was chosen for a class project.
- Talking points: trade-off of `EnsureCreated` vs. full migrations for a project this
  size, where the `.db` file lives.
- Reference: `src/TruthLens.Api/Program.cs` (startup block), `src/TruthLens.Api/Data/`.

### 12. Deployment & demo wrap-up — Richmond
Show the live deployed app (both links from the submission form) working end to end —
run through all three features once quickly — then close out.
- Talking points: how the API and client are deployed separately (Docker container for
  the API, static hosting for the Blazor client), CORS between the two, what you'd add
  next with more time (bigger training set, richer source directory, user accounts).
- Reference: `README.md` → "Deployment", `src/TruthLens.Api/Dockerfile`,
  `docs/DEPLOYMENT_CHECKLIST.md`.

---

## Before you record

- [ ] Everyone has picked a slice above and filled in their name.
- [ ] The app is actually deployed (see `docs/DEPLOYMENT_CHECKLIST.md`) — the video should
      show the *real* deployed links, not just `localhost`.
- [ ] Each person has opened and briefly run **their own slice** of code locally at least
      once — don't just read this doc on camera.
- [ ] Video uploaded to YouTube as **Unlisted** (not Private), and plays back correctly in
      an incognito window before submitting.
