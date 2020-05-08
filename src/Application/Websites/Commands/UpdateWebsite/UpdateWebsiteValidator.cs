using FluentValidation;
using WebsiteManagement.Application.Websites.Commands.Abstract;

namespace WebsiteManagement.Application.Websites.Commands.UpdateWebsite
{
    public class UpdateWebsiteValidator : AbstractValidator<UpdateWebsite>
    {
        public UpdateWebsiteValidator()
        {
            Include(new WebsiteBaseValidator());
        }
    }
}