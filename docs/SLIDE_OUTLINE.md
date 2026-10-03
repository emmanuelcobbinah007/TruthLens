# TruthLens — Presentation Slide Outline

Written for: the 12 team members presenting slides to the class, followed by a live demo.

Presenter numbers match the roles in `PRESENTATION_GUIDE.md`. Replace each `Presenter N` with the member's name.

Target: about 14 slides, roughly 10 minutes of talk, then a 5-minute demo.

---

## Slides

1. **Title: TruthLens, a misinformation screener for Ghana** — *Presenter 1*
2. **The problem** — Health and election misinformation spreads faster than fact-checks. Add a local example, such as a fake health remedy or a false election claim circulating in Ghana. — *Presenter 1*
3. **What TruthLens does** — Paste a claim, get a credibility signal and source context, and flag claims for the community. Show one screenshot of the claim checker. — *Presenter 2*
4. **Architecture** — Blazor WebAssembly frontend on Netlify, ASP.NET Core Web API on Render, ML.NET model and SQLite inside the API, shared DTOs in a common project. Draw the flow of one request from browser to model and back. — *Presenter 2*
5. **Why two deployments** — Frontend and backend are separate projects, so each has its own link. Explain CORS briefly: the API only accepts requests from the Netlify site. — *Presenter 3*
6. **The ML model** — Text featurization, then an SDCA maximum-entropy classifier, trained at startup. Explain what the three labels mean and the confidence score. — *Presenter 4*
7. **Training data** — About 75 hand-labeled example claims across health and election topics. Show two examples per label. — *Presenter 5*
8. **Claim checker, end to end** — Show the frontend page and the `POST /api/claims/check` endpoint side by side. — *Presenter 6*
9. **Source reputation** — The Ghanaian directory: news outlets, government and health bodies, and fact-checkers, with the categories explained. Note that categories for outlets are judgment calls. — *Presenter 7*
10. **Source lookup, backend** — How domains are normalized and how unknown domains are handled instead of erroring. — *Presenter 8*
11. **Community flagging** — The flag form, the flags feed, and how repeat flags on the same claim are grouped. — *Presenter 9*
12. **Data and persistence** — EF Core with SQLite, how the database is created and seeded, and why it resets on each Render redeploy. — *Presenter 10*
13. **Deployment** — Docker for the API on Render, a static build on Netlify, and the environment variables that connect them. — *Presenter 11*
14. **Limitations and next steps** — Small training set, small source list, no live updates, not a substitute for fact-checking. Next steps: a larger local dataset, user accounts, and a persistent database. — *Presenter 12*

---

## Demo (about 5 minutes, after the slides)

- Open the live Netlify site in a fresh tab: https://truthlens-frontend.netlify.app/
- Check a known misinformation claim, then a credible one, to show the difference in the signal.
- Use the source lookup with `graphic.com.gh` and `ghanaweb.com`.
- Flag a claim and show it on the Community flags page.
- Keep a backup: a screen recording of the demo, in case the free Render instance is asleep or the network fails.

## Before presenting

- Warm up the Render API by opening https://truthlens-backend-6lt0.onrender.com/api/sources a minute before you start.
- Assign one person to run the demo, so the speaker doesn't have to switch between slides and the browser.
- Keep each slide to one idea and one visual. Code slides should show the key method or endpoint, not whole files.
