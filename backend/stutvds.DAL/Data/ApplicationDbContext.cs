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
        // public DbSet<QuestionEntity> Questions { get; set; }
        // public DbSet<TwisterTemplateEntity> TwisterTemplates { get; set; }
        // public DbSet<TriggerTemplateEntity> TriggerTemplates { get; set; }

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

            // modelBuilder.Entity<ApplicationUser>()
            //.HasMany( x => x.Triggers)
            //.WithOne(f => f.ApplicationUser)
            //.HasForeignKey(p => p.ApplicationUserId);

            // modelBuilder.Entity<TriggerEntity>()
            //     .HasOne(p => p.ApplicationUser)
            //     .WithMany(b => b.Triggers);

            //modelBuilder.Entity<TriggerEntity>().Property(u => u.ApplicationUserId).HasColumnName("ApplicationUserId1");
        }
    }
}