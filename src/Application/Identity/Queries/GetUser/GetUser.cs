using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;

namespace WebsiteManagement.Application.Identity.Queries.GetUser
{
    public class GetUser : IRequest<OperationResult<UserOutputModel>>
    {
        public string Username { get; set; }

       
        public string Password { get; set; }
    }

    public class GetUserHandler : IRequestHandler<GetUser, OperationResult<UserOutputModel>>
    {
        private readonly IAuthenticationService _authenticationService;

        public GetUserHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public Task<OperationResult<UserOutputModel>> Handle(GetUser command, CancellationToken cancellationToken)
        {
            var user = _authenticationService.Authenticate(command.Username, command.Password);
            if (user is null)
            {
                return Task.FromResult(OperationResult<UserOutputModel>.Failure("Invalid user name or password"));
            }

            return Task.FromResult(OperationResult<UserOutputModel>.Success(user));
        }
    }
}
