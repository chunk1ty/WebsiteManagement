using System;

namespace WebsiteManagement.Domain
{
    public class Category
    {
        public int Id { get; set; }

        public Guid WebsiteId { get; set; }

        public string Value { get; set; }
    }
}
