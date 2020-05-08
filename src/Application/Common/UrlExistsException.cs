using System;

namespace WebsiteManagement.Application.Common
{
    public class UrlExistsException : Exception
    {
        public UrlExistsException()
        {
        }

        public UrlExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
