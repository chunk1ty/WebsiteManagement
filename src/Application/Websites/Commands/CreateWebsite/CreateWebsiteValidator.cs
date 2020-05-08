using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Application.Websites.Commands.Abstract;

namespace WebsiteManagement.Application.Websites.Commands.CreateWebsite
{
    public class CreateWebsiteValidator : WebsiteBaseValidator, IValidator<CreateWebsite>
    {
        public OperationResult<bool> IsValid(CreateWebsite request)
        {
            return base.IsValid(request);
        }
    }
}