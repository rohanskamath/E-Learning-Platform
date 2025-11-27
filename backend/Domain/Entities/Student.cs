namespace backend.Domain.Entities
{
    public class Student
    {
        public Guid StudentId { get; set; }
        public Guid FkUserId { get; set; }

        // Navigation Property
        public ApplicationUsers? User { get; set; }
    }
}
