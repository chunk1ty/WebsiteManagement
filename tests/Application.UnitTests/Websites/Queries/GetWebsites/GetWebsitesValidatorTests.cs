using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using NUnit.Framework;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Common.Behaviours;
using WebsiteManagement.Application.Websites;
using WebsiteManagement.Application.Websites.Queries.GetWebsites;

namespace WebsiteManagement.Application.UnitTests.Websites.Queries.GetWebsites
{
    [TestFixture]
    public class GetWebsitesValidatorTests
    {
        private ValidationBehavior<Application.Websites.Queries.GetWebsites.GetWebsites, OperationResult<List<WebsiteOutputModel>>> _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ValidationBehavior<Application.Websites.Queries.GetWebsites.GetWebsites, OperationResult<List<WebsiteOutputModel>>>(new GetWebsitesValidatorValidator());
        }

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
        public async Task Validate_WithCorrectOrderByClause_ShouldNotReturnValidationResult(string orderBy)
        {
            // Arrange
            RequestHandlerDelegate<OperationResult<List<WebsiteOutputModel>>> requestHandlerDelegate = () => Task.FromResult(OperationResult<List<WebsiteOutputModel>>.Success(null));

            // Act
            var query = new Application.Websites.Queries.GetWebsites.GetWebsites { PageNumber = 1, PageSize = 10, OrderBy = orderBy };
            var validationResult = await _validator.Handle(query, CancellationToken.None, requestHandlerDelegate);

            // Assert
            validationResult.IsSuccessful.Should().BeTrue();

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
        public async Task Validate_WithIncorrectOrderByClause_ShouldReturnValidationResult(string orderBy)
        {
            // Arrange
            RequestHandlerDelegate<OperationResult<List<WebsiteOutputModel>>> requestHandlerDelegate = () => Task.FromResult(OperationResult<List<WebsiteOutputModel>>.Success(null));

            // Act
            var query = new Application.Websites.Queries.GetWebsites.GetWebsites { PageNumber = 1, PageSize = 10, OrderBy = orderBy };
            var validationResult = await _validator.Handle(query, CancellationToken.None, requestHandlerDelegate);

            // Assert
            validationResult.IsSuccessful.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors.First().Key.Should().Be("OrderBy");
            validationResult.Errors.First().Value.Should().Be("Invalid OrderBy clause");
        }
    }
}
