using System;
using System.Collections.Generic;

namespace WebsiteManagement.Application.Websites
{
    public class WebsiteOutputModel
    {
        public WebsiteOutputModel(Guid id,
                                  string name,
                                  string url,
                                  List<string> categories,
                                  ImageOutputModel image,
                                  LoginOutputModel login)
        {
            Id = id;
            Name = name;
            Url = url;
            Categories = categories;
            Image = image;
            Login = login;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Url { get; }

        public List<string> Categories { get; }

        public ImageOutputModel Image { get; }

        public LoginOutputModel Login { get; }
    }

    public class ImageOutputModel
    {
        public ImageOutputModel(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string DownloadLink { get; set; }
    }

    public class LoginOutputModel
    {
        public LoginOutputModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }

        public string Password { get; }
    }
}
