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
        // only for Teacher
        public int? Experience { get; set; } 
        // only for Teacher
        public decimal? HourlyRate { get; set; } 
        public required string Role { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? TimeZone { get; set; }
    }
}
