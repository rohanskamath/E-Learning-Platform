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
                b.HasIndex(s => s.Email).IsUnique();
                b.HasIndex(s => s.PhoneNumber).IsUnique();
                b.HasIndex(s => s.FkUserId).IsUnique();

                b.HasOne<ApplicationUsers>()
                 .WithMany()
                 .HasForeignKey(s => s.FkUserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Teacher>(b =>
            {
                b.HasKey(t => t.TeacherId);
                b.HasIndex(t => t.Email).IsUnique();
                b.HasIndex(t => t.PhoneNumber).IsUnique();
                b.HasIndex(t => t.FkUserId).IsUnique();
                b.Property(t => t.HourlyRate).HasPrecision(18, 2);

                b.HasOne<ApplicationUsers>()
                 .WithMany()
                 .HasForeignKey(t => t.FkUserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
