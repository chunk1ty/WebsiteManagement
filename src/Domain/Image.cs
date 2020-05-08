using System;

namespace WebsiteManagement.Domain
{
    public class Image
    {
        public int Id { get; set; }

        public Guid WebsiteId { get; set; }

        public string Name { get; set; }

        public byte[] Blob { get; set; }

        public string MimeType { get; set; }
    }
}
