using Microsoft.AspNetCore.Identity;

namespace backend.Domain.Entities
{
    public class ApplicationUsers: IdentityUser<Guid>
    {
        // Remaining properties of ApplicationUsers class

        // Additional properties added below
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
