using System.IO;
using System.Linq;
using FluentValidation;

namespace WebsiteManagement.Application.Websites.Commands.Abstract
{
    internal class WebsiteBaseValidator : AbstractValidator<WebsiteManipulation>
    {
        private const long MaxImageSizeInMb = 5;
        private readonly string[] _supportedFileExtensions = { ".jpg", ".png", ".jpeg" };

        internal WebsiteBaseValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name is required.")
                                .Length(3, 100).WithMessage("Name should be more than 3 and less than 100 characters.");

            RuleFor(x => x.Url).NotNull().WithMessage("Url is required.")
                               .Length(3, 100).WithMessage("Url should be more than 3 and less than 100 characters");

            RuleFor(x => x.Categories).NotNull().WithMessage("Category is required.")
                                      .ChildRules(category =>
                                      {
                                          category.RuleForEach(x => x).NotNull().WithMessage("Category value is required.")
                                                                      .Length(3, 100).WithMessage("Category value should be more than 3 and less than 100 characters");
                                      });

            RuleFor(x => x.Image).Cascade(CascadeMode.StopOnFirstFailure)
                                 .NotNull().WithMessage("Image is required.")
                                 .Must(image => _supportedFileExtensions.Contains(Path.GetExtension(image.Name))).WithMessage($"File extension is not supported. Supported types   are: {string.Join(", ", _supportedFileExtensions)}")
                                 .Must(image => image.Blob.Length < MaxImageSizeInMb * 1024 * 1024).WithMessage($"File size is greater than {MaxImageSizeInMb} MB.");

            RuleFor(x => x.Email).NotNull().WithMessage("Password is required.")
                                 .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password).NotNull().WithMessage("Password is required.")
                                    .Length(3, 100).WithMessage("Password should be more than 3 and less than 100 characters");
        }
    }
}