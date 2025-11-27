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
            var user=new ApplicationUsers
            {
                UserName = request.Email,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user, request.Role);
            if(request.Role.ToLower()=="student")
            {

            }
        }
    }
}
