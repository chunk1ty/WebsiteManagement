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
using WebsiteManagement.Application.Websites.Commands.Abstract;
using WebsiteManagement.Application.Websites.Commands.CreateWebsite;
using WebsiteManagement.Common;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.UnitTests.Websites.Commands.CreateWebsite
{
    [TestFixture]
    public class CreateWebsiteHandlerTests
    {
        private Mock<IWebsiteRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ICypher> _cypherMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IWebsiteRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cypherMock = new Mock<ICypher>();
        }

        [Test]
        public async Task HandleAsync_WhenCommitAsyncThrowsUrlExistsException_ShouldReturnFailureResult()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.CommitAsync(CancellationToken.None)).Throws<UrlExistsException>();
            var handler = new CreateWebsiteHandler(_repositoryMock.Object, _unitOfWorkMock.Object, _cypherMock.Object);

            // Act
            var request = new Application.Websites.Commands.CreateWebsite.CreateWebsite("mySite", "www.mysite.com", new List<string> { "cat1,cat2" }, new ImageManipulation("myImage.png", "image/png", new byte[1]), "ank@ank.bg", "123456");
            OperationResult<GetWebsiteResponse> createWebsiteOperation = await handler.Handle(request, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(x => x.CommitAsync(CancellationToken.None), Times.Once);

            createWebsiteOperation.Should().BeOfType(typeof(OperationResult<GetWebsiteResponse>));
            createWebsiteOperation.IsSuccessful.Should().BeFalse();
            createWebsiteOperation.Errors.Count.Should().Be(1);
            createWebsiteOperation.Errors.First().Key.Should().Be("Url");
            createWebsiteOperation.Errors.First().Value.Should().Be("Url already exists.");
        }

        [Test]
        public async Task HandleAsync_WhenCommandIsCorrent_ShouldReturnSuccessResult()
        {
            // Arrange
            var handler = new CreateWebsiteHandler(_repositoryMock.Object, _unitOfWorkMock.Object, _cypherMock.Object);

            // Act
            var request = new Application.Websites.Commands.CreateWebsite.CreateWebsite("mySite", "www.mysite.com", new List<string> { "cat1", "cat2" }, new ImageManipulation("myImage.png", "image/png", new byte[1]), "ank@ank.bg", "123456");
            OperationResult<GetWebsiteResponse> createWebsiteOperation = await handler.Handle(request, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(x => x.CommitAsync(CancellationToken.None), Times.Once);
            _repositoryMock.Verify(x => x.Add(It.IsAny<Website>()), Times.Once);

            createWebsiteOperation.Should().BeOfType(typeof(OperationResult<GetWebsiteResponse>));
            createWebsiteOperation.IsSuccessful.Should().BeTrue();
            createWebsiteOperation.Errors.Should().BeNull();

            createWebsiteOperation.Result.Name.Should().Be("mySite");
            createWebsiteOperation.Result.Url.Should().Be("www.mysite.com");
            createWebsiteOperation.Result.Categories.Count.Should().Be(2);
            createWebsiteOperation.Result.Categories[0].Should().Be("cat1");
            createWebsiteOperation.Result.Categories[1].Should().Be("cat2");
            createWebsiteOperation.Result.Image.Name.Should().Be("myImage.png");
            createWebsiteOperation.Result.Image.DownloadLink.Should().BeNull();
            createWebsiteOperation.Result.Login.Email.Should().Be("ank@ank.bg");
            createWebsiteOperation.Result.Login.Password.Should().Be("123456");
        }
    }
}
