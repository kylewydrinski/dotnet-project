
using dotnet_project.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project.Data;

public class DotNetContext : DbContext
{
    public DotNetContext(
        DbContextOptions<DotNetContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username)
                .IsUnique();

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Property(u => u.Role)
                .HasDefaultValue("User");
        });
    }
}
