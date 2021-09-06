namespace WebsiteManagement.Application.Images.Queries.GetImage
{
    public class GetImageResponse
    {
        public GetImageResponse(string name, string contentType, byte[] blob)
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
