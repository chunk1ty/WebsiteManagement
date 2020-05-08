namespace WebsiteManagement.Infrastructure.Identity
{
    public class User
    {
        public User(int id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }

        public int Id { get; }

        public string Name { get; }

        // shouldn't be stored as plain text
        public string Password { get; }
    }
}