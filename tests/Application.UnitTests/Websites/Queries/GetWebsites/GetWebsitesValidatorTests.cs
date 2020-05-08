using FluentAssertions;
using NUnit.Framework;
using WebsiteManagement.Application.Websites.Queries.GetWebsites;

namespace WebsiteManagement.Application.UnitTests.Websites.Queries.GetWebsites
{
    class GetWebsitesValidatorTests
    {
        [TestCase("url")]
        [TestCase("uRl")]
        [TestCase("url desc")]
        [TestCase("email")]
        [TestCase("email desc")]
        [TestCase("Name")]
        [TestCase("NamE")]
        [TestCase("NamE   ")]
        [TestCase("  name")]
        [TestCase("  name asc")]
        [TestCase("  name desc  ")]
        public void Validate_WithCorrectOrderByClause_ShouldNotReturnValidationResult(string orderBy)
        {
            // Arrange
           var validator = new GetWebsitesValidator();

           // Act
           var query = new Application.Websites.Queries.GetWebsites.GetWebsites { PageNumber = 1, PageSize = 10, OrderBy = orderBy };
           var validationResult = validator.IsValid(query);

            // Assert
            validationResult.IsSuccessful.Should().BeTrue();
            validationResult.Result.Should().BeTrue();
        }

        [TestCase("url    asc")]
        [TestCase("uRl   desc")]
        [TestCase("email           desc")]
        [TestCase("category")]
        [TestCase("category asc")]
        [TestCase("password desc")]
        [TestCase("image")]
        [TestCase("1")]
        [TestCase("gfdgdfg asc")]
        [TestCase("asc")]
        [TestCase("desc")]
        public void Validate_WithIncorrectOrderByClause_ShouldReturnValidationResult(string orderBy)
        {
            // Arrange
            var validator = new GetWebsitesValidator();

            // Act
            var query = new Application.Websites.Queries.GetWebsites.GetWebsites { PageNumber = 1, PageSize = 10, OrderBy = orderBy };
            var validationResult = validator.IsValid(query);

            // Assert
            validationResult.IsSuccessful.Should().BeFalse();
            validationResult.ErrorMessage.Should().Be("Invalid OrderBy clause.");
        }
    }
}
