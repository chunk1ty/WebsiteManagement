using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Application.Websites;
using WebsiteManagement.Application.Websites.Queries.GetWebsites;
using WebsiteManagement.Common;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.UnitTests.Websites.Queries.GetWebsites
{
    [TestFixture]
    public class GetWebsitesHandlerTests
    {
        private Mock<IWebsiteRepository> _repositoryMock;
        private Mock<ICypher> _cypherMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IWebsiteRepository>();
            _cypherMock = new Mock<ICypher>();
        }

        [Test]
        public async Task GetAsync_WithCorrectQuery_ShouldReturnSuccessResult()
        {
            // Arrange
            _repositoryMock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                           .ReturnsAsync(new List<Website>());

            var handler = new GetWebsitesHandler(_repositoryMock.Object, _cypherMock.Object);

            // Act
            var query = new Application.Websites.Queries.GetWebsites.GetWebsitesRequest { PageNumber = 1, PageSize = 10, OrderBy = "name desc" };

            OperationResult<List<GetWebsiteResponse>> operationResult = await handler.Handle(query, CancellationToken.None);

            // Assert
            operationResult.Should().BeOfType(typeof(OperationResult<List<GetWebsiteResponse>>));
            operationResult.IsSuccessful.Should().BeTrue();
            operationResult.Result.Should().NotBeNull();

            _repositoryMock.Verify(x => x.GetAll(It.Is<int>(x => x == 1),
                                                 It.Is<int>(x => x == 10),
                                                 It.Is<string>(x => x == "name desc")));
        }
    }
}
