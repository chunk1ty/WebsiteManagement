namespace WebsiteManagement.Application.Interfaces
{
    public interface ICypher
    {
        string Encrypt(string value);

        string Decrypt(string value);
    }
}
