using WebsiteManagement.Application.Identity.Queries.GetUser;

namespace WebsiteManagement.Application.Interfaces
{
    public interface IAuthenticationService
    {
        LoginResponse Authenticate(string username, string password);
    }
}