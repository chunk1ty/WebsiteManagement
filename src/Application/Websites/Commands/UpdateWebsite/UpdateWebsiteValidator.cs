using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Application.Websites.Commands.Abstract;

namespace WebsiteManagement.Application.Websites.Commands.UpdateWebsite
{
    public class UpdateWebsiteValidator : WebsiteBaseValidator, IValidator<UpdateWebsite>
    {
        public OperationResult<bool> IsValid(UpdateWebsite request)
        {
            return base.IsValid(request);
        }
    }
}