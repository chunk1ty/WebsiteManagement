using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using WebsiteManagement.Application.Websites.Commands.Abstract;

namespace WebsiteManagement.Api.Models.Input
{
    public abstract class WebsiteManipulation
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public List<string> Categories { get; set; }

        public IFormFile Image { get; set; }

        public Login Login { get; set; }

        protected ImageManipulation GetImage()
        {
            return Image is null ? null : new ImageManipulation(Image.FileName, Image.ContentType, Convert(Image));
        }

        private byte[] Convert(IFormFile image)
        {
            if (image is null)
            {
                return null;
            }

            byte[] fileBlob;
            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                fileBlob = memoryStream.ToArray();
            }

            return fileBlob;
        }
    }

    public class Login
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
