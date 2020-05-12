using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebsiteManagement.Application.Identity.Queries.GetUser;
using WebsiteManagement.Application.Interfaces;

namespace WebsiteManagement.Infrastructure.Identity
{
    public class AuthenticationService : IAuthenticationService
    {
        public UserOutputModel Authenticate(string username, string password)
        {
            var user = IdentityInMemoryDbContext.Instance.Users.SingleOrDefault(u => u.Name == username &&
                                                                                     u.Password == password);
            if (user is null)
            {
                return null;
            }

            return new UserOutputModel(user.Id, user.Name, user.Password, GenerateJwtToken(user));
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("my secret keyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
