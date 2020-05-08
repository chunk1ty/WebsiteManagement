using WebsiteManagement.Application.Common;

namespace WebsiteManagement.Application.Interfaces
{
    public interface IValidator<TRequest>
    {
        OperationResult<bool> IsValid(TRequest request);
    }
}