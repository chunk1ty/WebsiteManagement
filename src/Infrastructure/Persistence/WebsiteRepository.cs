using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Infrastructure.Persistence
{
    public class WebsiteRepository : IWebsiteRepository
    {
        private readonly WebsiteManagementDbContext _context;

        public WebsiteRepository(WebsiteManagementDbContext context)
        {
            _context = context;
        }

        public void Add(Website website)
        {
            _context.Websites.Add(website);
        }

        public async Task<List<Website>> GetAll(int pageNumber, int pageSize, string orderBy)
        {
            IQueryable<Website> websiteCollection = _context.Websites;

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                websiteCollection = websiteCollection.OrderBy(orderBy);
            }
            
            return await websiteCollection.Include(w => w.Categories)
                                          .Include(w => w.Image)
                                          .Where(x => !x.IsDeleted)
                                          .Skip((pageNumber - 1) * pageSize)
                                          .Take(pageSize)
                                          .AsNoTracking().Select(w => new Website
                                          {
                                              Id = w.Id,
                                              Name = w.Name,
                                              Url = w.Url,
                                              Categories = w.Categories,
                                              Image = new Image
                                              {
                                                  Name = w.Image.Name
                                              },
                                              Email = w.Email,
                                              Password = w.Password
                                          })
                                          .ToListAsync();
        }

        public async Task<Website> GetByIdAsync(Guid id)
        {
            return await _context.Websites.Include(w => w.Categories)
                                          .Include(w => w.Image)
                                          .SingleOrDefaultAsync(x => x.Id == id && 
                                                                     !x.IsDeleted);
        }
    }
}
