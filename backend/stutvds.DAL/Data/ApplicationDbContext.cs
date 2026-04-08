using System.Linq;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StopStatAuth_6_0.Entities;
using stutvds.DAL.Entities;

namespace stutvds.Data
{
    public class ApplicationDbContext : IdentityDbContext, IDataProtectionKeyContext
    {
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public DbSet<TriggerEntity> Triggers { get; set; }
        public DbSet<ArticleEntity> Articles { get; set; }
        public DbSet<DayLesson> DayLessons { get; set; }
        public DbSet<VoiceAnalysisEntity> VoiceAnalyses { get; set; }
        public DbSet<Histogram> Histograms { get; set; }
        public DbSet<CharItem> CharItems { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

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
            
            modelBuilder.Entity<VoiceAnalysisEntity>()
                .Property(v => v.MfccJson)
                .HasColumnType("nvarchar(max)");
            
            modelBuilder.Entity<Histogram>()
                .HasMany(h => h.Chars)
                .WithOne(c => c.Histogram)
                .HasForeignKey(c => c.HistogramId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}