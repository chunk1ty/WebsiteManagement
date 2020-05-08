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
using WebsiteManagement.Application.Websites.Commands.Abstract;
using WebsiteManagement.Application.Websites.Commands.UpdateWebsite;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.UnitTests.Websites.Commands.UpdateWebsite
{
    [TestFixture]
    public class UpdateWebsiteHandlerTests
    {
        private Mock<IWebsiteRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<ICypher> _cyhperMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IWebsiteRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cyhperMock = new Mock<ICypher>();
        }

        [Test]
        public async Task HandleAsync_WhenWebsiteNotFound_ShouldReturnFailureResult()
        {
            // Arrange
            Website website = null;
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(website);

            var handler = new UpdateWebsiteHandler(_repositoryMock.Object, _unitOfWorkMock.Object, _cyhperMock.Object);

            // Act
            var command = new Application.Websites.Commands.UpdateWebsite.UpdateWebsite(Guid.Empty, "mySite", "www.mysite.com", new List<string> { "cat1", "cat2" }, new ImageManipulation("myImage.png", "image/png", new byte[1]), "ank@ank.bg", "123456");
            OperationResult<bool> operationResult = await handler.Handle(command, CancellationToken.None);

            // Assert
            operationResult.Should().BeOfType(typeof(OperationResult<bool>));
            operationResult.IsSuccessful.Should().BeFalse();
            operationResult.Errors.First().Key.Should().Be("WebsiteId");
            operationResult.Errors.First().Value.Should().Be(ErrorMessages.WebsiteNotFound);
        }

        [Test]
        public async Task HandleAsync_WhenCommitAsyncThrowsUrlExistsException_ShouldReturnFailureResult()
        {
            // Arrange
            Website website = new Website();
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(website);

            _unitOfWorkMock.Setup(x => x.CommitAsync(CancellationToken.None)).Throws<UrlExistsException>();

            var handler = new UpdateWebsiteHandler(_repositoryMock.Object, _unitOfWorkMock.Object, _cyhperMock.Object);

            // Act
            var command = new Application.Websites.Commands.UpdateWebsite.UpdateWebsite(Guid.Empty, "mySite", "www.mysite.com", new List<string> { "cat1", "cat2" }, new ImageManipulation("myImage.png", "image/png", new byte[1]), "ank@ank.bg", "123456");
            OperationResult<bool> operationResult = await handler.Handle(command, CancellationToken.None);

            // Assert
            operationResult.Should().BeOfType(typeof(OperationResult<bool>));
            operationResult.IsSuccessful.Should().BeFalse();
            operationResult.Errors.Count.Should().Be(1);
            operationResult.Errors.First().Key.Should().Be("Url");
            operationResult.Errors.First().Value.Should().Be("Url already exists.");
        }

        [Test]
        public async Task HandleAsync_WithCorrectCommand_ShouldReturnSuccessResult()
        {
            // Arrange
            Website website = new Website();
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(website);

            var handler = new UpdateWebsiteHandler(_repositoryMock.Object, _unitOfWorkMock.Object, _cyhperMock.Object);

            // Act
            var command = new Application.Websites.Commands.UpdateWebsite.UpdateWebsite(Guid.Empty, "mySite", "www.mysite.com", new List<string> { "cat1", "cat2" }, new ImageManipulation("myImage.png", "image/png", new byte[1]), "ank@ank.bg", "123456");
            OperationResult<bool> operationResult = await handler.Handle(command, CancellationToken.None);

            // Assert
            _unitOfWorkMock.Verify(x => x.CommitAsync(CancellationToken.None), Times.Once());

            operationResult.Should().BeOfType(typeof(OperationResult<bool>));
            operationResult.IsSuccessful.Should().BeTrue();
        }
    }
}
