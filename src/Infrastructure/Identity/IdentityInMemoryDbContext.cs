using System.Collections.Generic;

namespace WebsiteManagement.Infrastructure.Identity
{
    public sealed class IdentityInMemoryDbContext
    {
        private static IdentityInMemoryDbContext _context;

        private readonly List<User> _users = new List<User> { new User(1, "admin", "admin") };

        private IdentityInMemoryDbContext()
        {
        }

        public static IdentityInMemoryDbContext Instance
        {
            get
            {
                if (_context == null)
                {
                    _context = new IdentityInMemoryDbContext();
                }

                return _context;
            }
        }

        public List<User> Users => _users;
    }
}
