namespace backend.Domain.Entities
{
    public class Teacher
    {
        public Guid TeacherId { get; set; }
        public Guid FkUserId { get; set; }
        public int Experience { get; set; }
        public decimal HourlyRate { get; set; }

        // Navigation Property
        public ApplicationUsers? User { get; set; }

    }
}
