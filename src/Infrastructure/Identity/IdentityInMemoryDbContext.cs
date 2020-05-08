using System.Collections.Generic;

namespace WebsiteManagement.Infrastructure.Identity
{
    public sealed class IdentityInMemoryDbContext
    {
        private static IdentityInMemoryDbContext _context;

        private IdentityInMemoryDbContext()
        {
        }

        public static List<User> Users = new List<User> {new User(1, "admin", "admin")};

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
    }
}
