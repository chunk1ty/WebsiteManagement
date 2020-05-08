using System.Collections.Generic;
using WebsiteManagement.Application.Interfaces;

namespace WebsiteManagement.Application.Websites.Commands.Abstract
{
    public abstract class WebsiteManipulationCommand 
    {
        protected WebsiteManipulationCommand(string name,
                                             string url,
                                             List<string> categories,
                                             byte[] imageContent,
                                             string imageName,
                                             string imageMimeType,
                                             string email,
                                             string password)
        {
            Name = name;
            Url = url;
            Categories = categories;
            ImageContent = imageContent;
            ImageName = imageName;
            ImageMimeType = imageMimeType;
            Email = email;
            Password = password;
        }

        public string Name { get; }

        public string Url { get; }

        public List<string> Categories { get; }

        public byte[] ImageContent { get; }

        public string ImageName { get; }

        public string ImageMimeType { get; }

        public string Email { get; }

        public string Password { get; }
    }
}
