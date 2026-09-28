using Microsoft.EntityFrameworkCore;

namespace TruthLens.Api.Data;

public class TruthLensDbContext(DbContextOptions<TruthLensDbContext> options) : DbContext(options)
{
    public DbSet<SourceEntity> Sources => Set<SourceEntity>();
    public DbSet<FlaggedClaimEntity> FlaggedClaims => Set<FlaggedClaimEntity>();
    public DbSet<FlagEntity> Flags => Set<FlagEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SourceEntity>()
            .HasIndex(s => s.Domain)
            .IsUnique();

        modelBuilder.Entity<FlaggedClaimEntity>()
            .HasIndex(f => f.ClaimText);

        modelBuilder.Entity<FlagEntity>()
            .HasOne(f => f.FlaggedClaim)
            .WithMany(c => c.Flags)
            .HasForeignKey(f => f.FlaggedClaimId);
    }
}

public class SourceEntity
{
    public int Id { get; set; }
    public string Domain { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class FlaggedClaimEntity
{
    public int Id { get; set; }
    public string ClaimText { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public List<FlagEntity> Flags { get; set; } = new();
}

public class FlagEntity
{
    public int Id { get; set; }
    public int FlaggedClaimId { get; set; }
    public FlaggedClaimEntity? FlaggedClaim { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Note { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
