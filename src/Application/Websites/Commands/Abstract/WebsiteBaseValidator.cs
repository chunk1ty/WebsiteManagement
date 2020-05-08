using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WebsiteManagement.Application.Common;

namespace WebsiteManagement.Application.Websites.Commands.Abstract
{
    public abstract class WebsiteBaseValidator
    {
        private readonly string[] _supportedFileExtensions = { ".jpg", ".png", ".jpeg" };
        private const long MaxImageSizeInMb = 5;

        public virtual OperationResult<bool> IsValid(WebsiteManipulation request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return OperationResult<bool>.Failure($"{nameof(request.Name)} cannot be empty.");

            if (request.Name.Length <= 3)
                return OperationResult<bool>.Failure($"{nameof(request.Name)} should be more than 3 characters.");

            if (request.Name.Length >= 100)
                return OperationResult<bool>.Failure($"{nameof(request.Name)} should be less than 100 characters.");

            if (string.IsNullOrWhiteSpace(request.Url))
                return OperationResult<bool>.Failure($"{nameof(request.Url)} cannot be empty.");

            if (request.Url.Length <= 3)
                return OperationResult<bool>.Failure($"{nameof(request.Url)} should be more than 3 characters.");

            if (request.Url.Length >= 100)
                return OperationResult<bool>.Failure($"{nameof(request.Url)} should be less than 100 characters.");

            if (request.Categories is null)
                return OperationResult<bool>.Failure($"Categories cannot be empty.");

            foreach (var category in request.Categories)
            {
                if (string.IsNullOrWhiteSpace(category))
                    return OperationResult<bool>.Failure("Category value cannot be empty.");

                if (category.Length <= 3)
                    return OperationResult<bool>.Failure("Category value should be more than 3 characters.");

                if (category.Length >= 100)
                    return OperationResult<bool>.Failure("Category value should be less than 100 characters.");
            }

            if (request.ImageName is null)
            {
                return OperationResult<bool>.Failure($"Image cannot be empty. Please select image.");
            }

            string extension = Path.GetExtension(request.ImageName);
            if (!_supportedFileExtensions.Contains(extension))
                return OperationResult<bool>.Failure($"File extension is not supported. Supported types are: {string.Join(", ", _supportedFileExtensions)}");

            long size = request.ImageBlob.Length;
            long maxImageSizeIiBytes = MaxImageSizeInMb * 1024 * 1024;
            if (size > maxImageSizeIiBytes)
                return OperationResult<bool>.Failure($"File size is greater than {MaxImageSizeInMb} MB.");


            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return OperationResult<bool>.Failure($"{nameof(request.Password)} cannot be empty.");
            }

            if (request.Password.Length <= 3)
            {
                return OperationResult<bool>.Failure($"{nameof(request.Password)} should be more than 3 characters.");
            }

            if (request.Password.Length >= 20)
            {
                return OperationResult<bool>.Failure($"{nameof(request.Password)} should be less than 20 characters.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return OperationResult<bool>.Failure($"{nameof(request.Email)} cannot be empty.");
            }

            if (request.Email.Length <= 3)
            {
                return OperationResult<bool>.Failure($"{nameof(request.Email)} should be more than 3 characters.");
            }

            if (request.Email.Length >= 20)
            {
                return OperationResult<bool>.Failure($"{nameof(request.Email)} should be less than 20 characters.");
            }

            if (!IsValidEmail(request.Email))
            {
                return OperationResult<bool>.Failure($"{nameof(request.Email)} should be valid.");
            }

            return OperationResult<bool>.Success(true);
        }

        private bool IsValidEmail(string email)
        {
            // source: http://thedailywtf.com/Articles/Validating_Email_Addresses.aspx
            Regex rx = new Regex(
                @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
            return rx.IsMatch(email);
        }
    }
}