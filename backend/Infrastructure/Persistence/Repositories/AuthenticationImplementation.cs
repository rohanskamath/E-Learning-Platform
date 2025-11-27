using backend.Domain.Entities;
using backend.Domain.Interfaces__Ports_;
using backend.Infrastructure.Persistence.Configurations;

namespace backend.Infrastructure.Persistence.Repositories
{
    public class AuthenticationImplementation : IAuthentication
    {
        private readonly ApplicationDBContext _context;
        public AuthenticationImplementation(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }
        public async Task<Student> RegisterStudent(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Teacher> RegisterTeacher(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }
    }
}
