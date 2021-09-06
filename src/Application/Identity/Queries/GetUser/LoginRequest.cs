using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Common;

namespace WebsiteManagement.Application.Identity.Queries.GetUser
{
    public class LoginRequest : IRequest<OperationResult<LoginResponse>>
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class LoginRequestHandler : IRequestHandler<LoginRequest, OperationResult<LoginResponse>>
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginRequestHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public Task<OperationResult<LoginResponse>> Handle(LoginRequest command, CancellationToken cancellationToken)
        {
            LoginResponse user = _authenticationService.Authenticate(command.Username, command.Password);
            if (user is null)
            {
                return Task.FromResult(OperationResult<LoginResponse>.Failure(new Dictionary<string, string> { { "Login", "Invalid user name or password" } }));
            }

            return Task.FromResult(OperationResult<LoginResponse>.Success(user));
        }
    }
}
