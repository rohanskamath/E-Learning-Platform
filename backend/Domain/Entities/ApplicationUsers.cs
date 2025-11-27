using Microsoft.AspNetCore.Identity;

namespace backend.Domain.Entities
{
    public class ApplicationUsers: IdentityUser<Guid>
    {
        // Remaining properties of ApplicationUsers class
        // Id, UserName, NormalizedUserName, Email, NormalizedEmail, PasswordHash, PhoneNumber

        // Additional properties added below
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public int Status { get; set; } = 1;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
    }
}
