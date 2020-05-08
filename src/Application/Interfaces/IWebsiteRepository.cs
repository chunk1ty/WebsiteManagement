using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.Interfaces
{
    public interface IWebsiteRepository
    {
        public void Add(Website website);

        Task<List<Website>> GetAll(int pageNumber, int pageSize, string orderBy);

        Task<Website> GetByIdAsync(Guid id);
    }
}
