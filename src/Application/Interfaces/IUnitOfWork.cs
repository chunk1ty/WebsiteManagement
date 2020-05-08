using System.Threading;
using System.Threading.Tasks;

namespace WebsiteManagement.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken);
    }
}
