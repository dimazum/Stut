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
        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<TriggerEntity> Triggers { get; set; }
        public DbSet<ArticleEntity> Articles { get; set; }
        public DbSet<DayLesson> DayLessons { get; set; }
        public DbSet<VoiceAnalysisEntity> VoiceAnalyses { get; set; }
        public DbSet<Histogram> Histograms { get; set; }
        public DbSet<CharItem> CharItems { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<LearnerTeacher> LearnerTeachers { get; set; }

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
            
            modelBuilder.Entity<LearnerTeacher>()
                .HasKey(x => new { x.LearnerId, x.TeacherId });

            modelBuilder.Entity<LearnerTeacher>()
                .HasOne(x => x.Learner)
                .WithMany(x => x.MyTeachers)
                .HasForeignKey(x => x.LearnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LearnerTeacher>()
                .HasOne(x => x.Teacher)
                .WithMany(x => x.MyLearners)
                .HasForeignKey(x => x.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<ChatMessage>()
                .HasIndex(m => new { m.SenderId, m.ReceiverId, m.SentAt })
                .HasDatabaseName("IX_ChatMessages_Dialog_SentAt")
                .IsDescending(false, false, true);
        }
    }
}