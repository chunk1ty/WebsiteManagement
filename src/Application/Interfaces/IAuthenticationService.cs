using WebsiteManagement.Application.Identity.Queries.GetUser;

namespace WebsiteManagement.Application.Interfaces
{
    public interface IAuthenticationService
    {
        UserOutputModel Authenticate(string username, string password);
    }
}