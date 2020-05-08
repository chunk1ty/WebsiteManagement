using System;
using System.Collections.Generic;

namespace WebsiteManagement.Domain
{
    public class Website
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
               
        public string Url { get; set; }

        public List<Category> Categories { get; set; }

        public Image Image { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsDeleted { get; set; }

        public void Update(Website webSite)
        {
            Name = webSite.Name;
            Url = webSite.Url;
            Categories = webSite.Categories;
            Image = webSite.Image;
            Email = webSite.Email;
            Password = webSite.Password;
        }
    }
}
