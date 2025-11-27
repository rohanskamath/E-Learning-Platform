namespace backend.Application.DTOs.AuthenticationDTOs
{
    public class TeacherDto
    {
        public Guid TeacherId { get; set; }
        public Guid FkUserId { get; set; }
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; } = null;
        public int Experience { get; set; }
        public decimal HourlyRate { get; set; }
        public int Status { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
