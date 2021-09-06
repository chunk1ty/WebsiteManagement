using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace WebsiteManagement.Application.Websites.Queries.GetWebsites
{
    public class GetWebsitesValidator : AbstractValidator<GetWebsitesRequest>
    {
        private static readonly List<string> Properties = new List<string> { "Name", "Url", "Email" };
        private static readonly List<string> Orders = new List<string> { string.Empty, "asc", "desc" };

        public GetWebsitesValidator()
        {
            RuleFor(x => x.OrderBy).Must(IsValid).WithMessage("Invalid OrderBy clause");
        }

        private bool IsValid(string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return true;
            }

            string[] fieldsAfterSplit = orderBy.Split(',');

            foreach (string field in fieldsAfterSplit)
            {
                string trimmedField = field.Trim();

                int indexOfFirstSpace = trimmedField.IndexOf(" ", StringComparison.Ordinal);
                string property = indexOfFirstSpace == -1 ? trimmedField :
                    trimmedField.Remove(indexOfFirstSpace);

                if (!Properties.Contains(property, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }

                string order = indexOfFirstSpace == -1 ? string.Empty : trimmedField.Substring(indexOfFirstSpace + 1);
                if (!Orders.Contains(order, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
