
using dotnet_project.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project.Data;

public class DotNetContext : DbContext
{
    public DotNetContext(DbContextOptions<DotNetContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Term> Terms => Set<Term>();
    public DbSet<StudyGuide> StudyGuides => Set<StudyGuide>();
    public DbSet<StudyGuideTerm> StudyGuideTerms => Set<StudyGuideTerm>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Username).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Role).HasDefaultValue("User");
            e.Property(u => u.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.Property(c => c.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasOne(c => c.User).WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Term>(e =>
        {
            e.Property(t => t.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasIndex(t => t.TermName); // not unique: duplicate terms allowed
            e.HasOne(t => t.User).WithMany(u => u.Terms)
                .HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(t => t.Category).WithMany(c => c.Terms)
                .HasForeignKey(t => t.CategoryId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<StudyGuide>(e =>
        {
            e.Property(s => s.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasOne(s => s.User).WithMany(u => u.StudyGuides)
                .HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StudyGuideTerm>(e =>
        {
            e.HasKey(x => new { x.StudyGuideId, x.TermId });
            e.HasOne(x => x.StudyGuide).WithMany(s => s.StudyGuideTerms)
                .HasForeignKey(x => x.StudyGuideId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Term).WithMany(t => t.StudyGuideTerms)
                .HasForeignKey(x => x.TermId).OnDelete(DeleteBehavior.NoAction);
        });
    }
}