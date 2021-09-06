namespace WebsiteManagement.Application.Identity.Queries.GetUser
{
    public class LoginResponse
    {
        public LoginResponse(int id, string username, string password, string token)
        {
            Id = id;
            Username = username;
            Password = password;
            Token = token;
        }

        public int Id { get; }

        public string Username { get; }

        public string Password { get; }

        public string Token { get; }
    }
}
