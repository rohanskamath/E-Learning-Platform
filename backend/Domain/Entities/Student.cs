namespace backend.Domain.Entities
{
    public class Student
    {
        public Guid StudentId { get; set; }
        public Guid FkUserId { get; set; }
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; } = null;
        public int Level { get; set; }
        public required string TimeZone { get; set; }   
        public int Status { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
