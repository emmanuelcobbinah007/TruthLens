using Microsoft.EntityFrameworkCore;
using TruthLens.Api.Data;
using TruthLens.Api.ML;

var builder = WebApplication.CreateBuilder(args);

const string ClientCorsPolicy = "TruthLensClient";

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("TruthLensDb") ?? "Data Source=truthlens.db";
builder.Services.AddDbContext<TruthLensDbContext>(options => options.UseSqlite(connectionString));

// Training the small in-memory dataset takes well under a second, so it is done once
// when this singleton is first resolved rather than shipping a separate model file.
builder.Services.AddSingleton<ClaimClassifierService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(ClientCorsPolicy, policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedClientOrigins").Get<string[]>()
            ?? ["https://localhost:7134", "http://localhost:5185"];

        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TruthLensDbContext>();
    db.Database.EnsureCreated();

    if (!db.Sources.Any())
    {
        db.Sources.AddRange(SeedData.Sources);
        db.SaveChanges();
    }

    // Warm up the classifier at startup instead of on the first request.
    scope.ServiceProvider.GetRequiredService<ClaimClassifierService>();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors(ClientCorsPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();
