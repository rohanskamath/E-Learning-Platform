using MediatR;
using System;

namespace backend.Application.Commands.Authentication
{
    public class RegisterCommand : IRequest<Guid>
    {
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}
