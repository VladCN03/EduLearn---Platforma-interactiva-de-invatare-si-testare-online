using Microsoft.EntityFrameworkCore;
using EducationalPlatform_Backend.Models;

namespace EducationalPlatform_Backend.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<CompletedLesson> CompletedLessons { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Key compus pentru CompletedLesson
            modelBuilder.Entity<CompletedLesson>()
                .HasKey(cl => new { cl.UserID, cl.LessonID });

            modelBuilder.Entity<CompletedLesson>()
                .HasOne(cl => cl.User)
                .WithMany(u => u.CompletedLessons)
                .HasForeignKey(cl => cl.UserID);

            modelBuilder.Entity<CompletedLesson>()
                .HasOne(cl => cl.Lesson)
                .WithMany(l => l.CompletedLessons)
                .HasForeignKey(cl => cl.LessonID);

            // Key compus pentru TestResult
            modelBuilder.Entity<TestResult>()
                .HasKey(tr => new { tr.UserID, tr.TestID });

            modelBuilder.Entity<TestResult>()
                .HasOne(tr => tr.User)
                .WithMany(u => u.TestResults)
                .HasForeignKey(tr => tr.UserID);

            modelBuilder.Entity<TestResult>()
                .HasOne(tr => tr.Test)
                .WithMany(t => t.TestResults)
                .HasForeignKey(tr => tr.TestID);
        }
    }
}
