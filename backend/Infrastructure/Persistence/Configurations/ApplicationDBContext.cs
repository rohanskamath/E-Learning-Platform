using backend.Domain;
using backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Persistence.Configurations
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUsers, IdentityRole<Guid>, Guid>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Fluent API configurations can be added here if needed
            builder.Entity<Student>(b =>
            {
                b.HasKey(s => s.StudentId);
                b.HasIndex(s => s.FkUserId).IsUnique();
                b.HasOne(s => s.User)
                 .WithOne(u => u.Student)
                 .HasForeignKey<Student>(s => s.FkUserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Teacher>(b =>
            {
                b.HasKey(t => t.TeacherId);
                b.HasIndex(t => t.FkUserId).IsUnique();
                b.Property(t => t.HourlyRate).HasPrecision(18, 2);

                b.HasOne(t => t.User)
                 .WithOne(u => u.Teacher)
                 .HasForeignKey<Teacher>(t => t.FkUserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
