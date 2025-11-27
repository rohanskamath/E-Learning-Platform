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
            // Basic input validation
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.Role))
            {
                throw new ArgumentException("Email, password and role are required.");
            }

            var role = request.Role.Trim();

            // Create User in Identity System
            var user = new ApplicationUsers
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
            };
            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var message = string.Join("; ", createResult.Errors.Select(e => $"{e.Code}: {e.Description}"));
                throw new InvalidOperationException($"User creation failed. {message}");
            }

            // Assign Role to User
            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                var message = string.Join("; ", roleResult.Errors.Select(e => $"{e.Code}: {e.Description}"));
                // Cleanup created user if role assignment fails
                await _userManager.DeleteAsync(user);
                throw new InvalidOperationException($"Role assignment failed. {message}");
            }

            if (role.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                var student = new Student
                {
                    StudentId = Guid.NewGuid(),
                    FkUserId = user.Id,
                };
                await _authentication.RegisterStudent(student);
                return student.StudentId;
            }
            else if (role.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
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
                throw new ArgumentException("Role must be either 'Student' or 'Teacher'.");
            }
        }
    }
}
