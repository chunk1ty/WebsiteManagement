using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Application.Websites;
using WebsiteManagement.Application.Websites.Queries.GetWebsite;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.UnitTests.Websites.Queries.GetWebsite
{
    [TestFixture]
    public class GetWebsiteHandlerTests
    {
        private Mock<IWebsiteRepository> _repositoryMock;
        private Mock<ICypher> _cyhperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IWebsiteRepository>();
            _cyhperMock = new Mock<ICypher>();
        }

        [Test]
        public async Task GetAsync_WhenWebsiteNotFound_ShouldReturnFailureResult()
        {
            // Arrange
            Website website = null;
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(website);
            var handler = new GetWebsiteHandler(_repositoryMock.Object, _cyhperMock.Object);

            // Act
            var query = new Application.Websites.Queries.GetWebsite.GetWebsite(Guid.Empty);
            OperationResult<WebsiteOutputModel> operationResult = await handler.Handle(query, CancellationToken.None);

            // Assert
            operationResult.Should().BeOfType(typeof(OperationResult<WebsiteOutputModel>));
            operationResult.IsSuccessful.Should().BeFalse();
            operationResult.Errors.First().Key.Should().Be("WebsiteId");
            operationResult.Errors.First().Value.Should().Be(ErrorMessages.WebsiteNotFound);
        }

        [Test]
        public async Task GetAsync_WithCorrectQuery_ShouldReturnSuccessResult()
        {
            // Arrange
            var website = new Website()
            {
                Name = "myWebsite",
                Url = "www.mysite.com",
                Password = "dTEknBrlH8Wy5+tYfm6teQ==",
                Email = "ank@ank.bg",
                Image = new Image()
                {
                    Name = "myImage.png",
                    Blob = new byte[17],
                    MimeType = "image/png"
                },
                Categories = new List<Category> { new Category { Value = "category 1" }, new Category() { Value = "category 2" } }
            };
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(website);
            var handler = new GetWebsiteHandler(_repositoryMock.Object, _cyhperMock.Object);

            // Act
            var query = new Application.Websites.Queries.GetWebsite.GetWebsite(Guid.Empty);
            OperationResult<WebsiteOutputModel> operationResult = await handler.Handle(query, CancellationToken.None);

            // Assert
            operationResult.Should().BeOfType(typeof(OperationResult<WebsiteOutputModel>));
            operationResult.IsSuccessful.Should().BeTrue();
        }
    }
}
