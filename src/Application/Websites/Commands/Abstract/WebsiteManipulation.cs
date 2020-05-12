using System.Collections.Generic;
using System.Linq;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.Websites.Commands.Abstract
{
    public abstract class WebsiteManipulation
    {
        protected WebsiteManipulation(string name,
                                      string url,
                                      List<string> categories,
                                      ImageManipulation image,
                                      string email,
                                      string password)
        {
            Name = name;
            Url = url;
            Categories = categories;
            Image = image;
            Email = email;
            Password = password;
        }

        public string Name { get; }

        public string Url { get; }

        public List<string> Categories { get; }

        public ImageManipulation Image { get; }

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
                    Name = Image.Name,
                    Blob = Image.Blob,
                    MimeType = Image.ContentType,
                },
                Email = Email,
                Password = encryptedPassword,
            };
        }
    }

    public class ImageManipulation
    {
        public ImageManipulation(string name, string contentType, byte[] blob)
        {
            Name = name;
            ContentType = contentType;
            Blob = blob;
        }

        public string Name { get; }

        public string ContentType { get; }

        public byte[] Blob { get; }
    }
}
