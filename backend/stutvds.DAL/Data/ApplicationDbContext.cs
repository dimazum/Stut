using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using StopStatAuth_6_0.Entities;
using stutvds.DAL.Entities;

namespace stutvds.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<TriggerEntity> Triggers { get; set; }
        public DbSet<ArticleEntity> Articles { get; set; }
        public DbSet<DayLesson> DayLessons { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ArticleEntity>()
                .Property(e => e.Content)
                .HasMaxLength(200000);
        }
    }
}