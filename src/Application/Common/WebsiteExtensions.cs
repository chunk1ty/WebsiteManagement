using System.Linq;
using WebsiteManagement.Application.Websites;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.Common
{
    public static class WebsiteExtensions
    {
        public static GetWebsiteResponse ToWebsiteOutputModel(this Website website, string decryptedPassword)
        {
            return new GetWebsiteResponse(website.Id,
                                          website.Name,
                                          website.Url,
                                          website.Categories.Select(x => x.Value).ToList(),
                                          new ImageOutputModel(website.Image.Name),
                                          new LoginOutputModel(website.Email, decryptedPassword));
        }
    }
}
