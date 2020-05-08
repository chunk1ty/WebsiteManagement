using System;
using WebsiteManagement.Application.Websites.Commands.UpdateWebsite;

namespace WebsiteManagement.Api.Models.Input
{
    public class UpdateWebsiteInputModel : WebsiteManipulation
    {
        public UpdateWebsite ToUpdateWebsite(Guid websiteId)
        {
            byte[] imageContent = Convert(Image);

            return new UpdateWebsite(websiteId,
                                    Name,
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
