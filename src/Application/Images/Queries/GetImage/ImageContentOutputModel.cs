namespace WebsiteManagement.Application.Images.Queries.GetImage
{
    public class ImageContentOutputModel
    {
        public ImageContentOutputModel(string name, string contentType, byte[] blob)
        {
            Name = name;
            ContentType = contentType;
            Blob = blob;
        }

        public string Name { get; }

        public string ContentType { get; set; }

        public byte[] Blob { get; }
    }
}
