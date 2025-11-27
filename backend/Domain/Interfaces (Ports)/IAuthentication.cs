using backend.Domain.Entities;

namespace backend.Domain.Interfaces__Ports_
{
    public interface IAuthentication
    {
        public Task<Student> RegisterStudent(Student student);
        public Task<Teacher> RegisterTeacher(Teacher teacher);

    }
}
