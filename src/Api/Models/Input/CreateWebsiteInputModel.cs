using WebsiteManagement.Application.Websites.Commands.CreateWebsite;

namespace WebsiteManagement.Api.Models.Input
{
    public  class CreateWebsiteInputModel : WebsiteManipulation
    {
        public CreateWebsite ToCreateWebsite()
        {
            byte[] imageContent =  Convert(Image);

            return new CreateWebsite(Name,
                                     Url,
                                     Categories,
                                     Image?.FileName,
                                     Image?.ContentType,
                                     imageContent,
                                     Login.Email,
                                     Login.Password);
        }
    }
}
