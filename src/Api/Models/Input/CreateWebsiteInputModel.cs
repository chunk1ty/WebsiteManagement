using WebsiteManagement.Application.Websites.Commands.CreateWebsite;

namespace WebsiteManagement.Api.Models.Input
{
    public  class CreateWebsiteInputModel : WebsiteManipulation
    {
        public CreateWebsite ToCreateWebsite()
        {
            return new CreateWebsite(Name,
                                     Url,
                                     Categories,
                                     GetImage(),
                                     Login.Email,
                                     Login.Password);
        }
    }
}
