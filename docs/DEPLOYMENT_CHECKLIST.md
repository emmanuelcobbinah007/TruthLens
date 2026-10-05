# Deployment checklist

The submission rubric requires, for a separate frontend/backend project, **GitHub +
deployment links for both**. Do these in order — the client needs the API's live URL
before it can be built for production.

## 1. Push to GitHub

```bash
cd TruthLens
git remote add origin <your-repo-url>
git push -u origin main
```

Make sure the repo is **public** or that graders have been given access — the rubric
requires "your GitHub repo is accessible"...

## 2. Deploy the API first

Pick any host that runs a Docker container or a .NET process (Render, Railway, Fly.io,
Azure App Service all have free/low-cost tiers). Using the included Dockerfile:

```bash
docker build -f src/TruthLens.Api/Dockerfile -t truthlens-api .
```

Before deploying, edit `src/TruthLens.Api/appsettings.Production.json` and replace
`REPLACE-WITH-YOUR-DEPLOYED-BLAZOR-CLIENT-URL` with the URL you'll deploy the client to
in step 3 (you can predict most static hosts' URL before the first deploy, e.g. a Netlify
site name or GitHub Pages URL — or deploy once, get the URL, then redeploy the API with it
filled in).

Note the API's live base URL once deployed, e.g. `https://truthlens-api.onrender.com`.

## 3. Deploy the client

Edit `src/TruthLens.Client/wwwroot/appsettings.Production.json` and replace
`ApiBaseUrl` with the API's live URL from step 2 (keep the trailing slash).

```bash
dotnet publish src/TruthLens.Client/TruthLens.Client.csproj -c Release -o publish
```

Deploy the contents of `publish/wwwroot` to a static host (Azure Static Web Apps,
Netlify, GitHub Pages, Cloudflare Pages).

## 4. Verify before submitting

- [ ] API deployment link opens and returns JSON, e.g. `GET <api-url>/api/sources`.
- [ ] Client deployment link opens in an **incognito window** and the claim checker
      returns a result (confirms CORS is configured correctly between the two).
- [ ] Both GitHub repo and both deployment links are in the group leader's submission form.
