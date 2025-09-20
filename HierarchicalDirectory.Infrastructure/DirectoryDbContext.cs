using Microsoft.EntityFrameworkCore;
using HierarchicalDirectory.Domain;

namespace HierarchicalDirectory.Infrastructure
{
    public class DirectoryDbContext : DbContext
    {
        public DirectoryDbContext(DbContextOptions<DirectoryDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Category>()
                .HasIndex(c => new { c.ParentId, c.Name })
                .IsUnique();
        }
    }
}
