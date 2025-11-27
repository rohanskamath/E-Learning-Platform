using AutoMapper;
using backend.Domain.Entities;
using backend.Domain.Interfaces__Ports_;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace backend.Application.Commands.Authentication
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, Guid>
    {
        public readonly UserManager<ApplicationUsers> _userManager;
        private readonly IAuthentication _authentication;
        private readonly IMapper _mapper;
        public RegisterHandler(UserManager<ApplicationUsers> userManager, IAuthentication authentication, IMapper mapper)
        {
            _authentication = authentication;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Create User in Identity System
            var user = new ApplicationUsers
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            // Assign Role to User
            await _userManager.AddToRoleAsync(user, request.Role);
            if (request.Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Registering Student...");
                var student = new Student
                {
                    StudentId = Guid.NewGuid(),
                    FkUserId = user.Id,
                };
                await _authentication.RegisterStudent(student);
                return student.StudentId;
            }
            else if (request.Role.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
            {
                var teacher = new Teacher
                {
                    TeacherId = Guid.NewGuid(),
                    FkUserId = user.Id,
                };
                await _authentication.RegisterTeacher(teacher);
                return teacher.TeacherId;
            }
            else
            {
                await _userManager.DeleteAsync(user);
                return Guid.Empty;
            }
        }
    }
}
