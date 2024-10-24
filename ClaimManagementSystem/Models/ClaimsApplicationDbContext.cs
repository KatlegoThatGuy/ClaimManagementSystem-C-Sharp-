using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class ClaimsApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ClaimsApplicationDbContext(DbContextOptions<ClaimsApplicationDbContext> options) : base(options) { }

    public DbSet<Claim> Claims { get; set; }

    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Claim>(entity =>
        {
            // Specify the precision and scale for HourlyRate
            entity.Property(e => e.HourlyRate)
                .HasColumnType("decimal(18, 2)"); // Adjust precision and scale as needed
        });

        base.OnModelCreating(modelBuilder);
    }
}