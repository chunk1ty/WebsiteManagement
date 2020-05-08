using System.Collections.Generic;
using System.Linq;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.Websites.Commands.Abstract
{
    public abstract class WebsiteManipulation //: IRequest
    {
        protected WebsiteManipulation(string name, 
                                      string url, 
                                      List<string> categories, 
                                      string imageName, 
                                      string imageContentType, 
                                      byte[] imageBlob, 
                                      string email, 
                                      string password)
        {
            Name = name;
            Url = url;
            Categories = categories;
            ImageName = imageName;
            ImageContentType = imageContentType;
            ImageBlob = imageBlob;
            Email = email;
            Password = password;
        }

        public string Name { get; }

        public string Url { get; }

        public List<string> Categories { get; }

        public string ImageName { get; }

        public string ImageContentType { get; }

        public byte[] ImageBlob { get; }

        public string Email { get; }

        public string Password { get; }

        public Website ToWebsite(string encryptedPassword)
        {
            return new Website
            {
                Name = Name,
                Url = Url,
                Categories = Categories.Select(c => new Category { Value = c }).ToList(),
                Image = new Image
                {
                    Blob = ImageBlob,
                    Name = ImageName,
                    MimeType = ImageContentType
                },
                Email = Email,
                Password = encryptedPassword
            };
        }
    }
}
