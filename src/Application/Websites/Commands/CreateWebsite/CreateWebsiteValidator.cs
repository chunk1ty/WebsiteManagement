using FluentValidation;
using WebsiteManagement.Application.Websites.Commands.Abstract;

namespace WebsiteManagement.Application.Websites.Commands.CreateWebsite
{
    public class CreateWebsiteValidator : AbstractValidator<CreateWebsite>
    {
        public CreateWebsiteValidator()
        {
            Include(new WebsiteBaseValidator());
        }
    }
}