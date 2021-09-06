using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Application.Websites.Commands.DeleteWebsite;
using WebsiteManagement.Common;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.UnitTests.Websites.Commands.DeleteWebsite
{
    [TestFixture]
    public class DeleteWebsiteHandlerTests
    {
        private Mock<IWebsiteRepository> _repositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<IWebsiteRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Test]
        public async Task HandleAsync_WhenWebsiteNotFound_ShouldReturnFailureResult()
        {
            // Arrange
            Website website = null;
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(website);

            var handler = new DeleteWebsiteHandler(_repositoryMock.Object, _unitOfWorkMock.Object);

            // Act
            var command = new Application.Websites.Commands.DeleteWebsite.DeleteWebsite(Guid.Empty);
            OperationResult<bool> operationResult = await handler.Handle(command, CancellationToken.None);

            // Assert
            operationResult.Should().BeOfType(typeof(OperationResult<bool>));
            operationResult.IsSuccessful.Should().BeFalse();
            operationResult.Errors.Count.Should().Be(1);
            operationResult.Errors.First().Key.Should().Be("WebsiteId");
            operationResult.Errors.First().Value.Should().Be(ErrorMessages.WebsiteNotFound);
        }

        [Test]
        public async Task HandleAsync_WithCorrectCommand_ShouldReturnSuccessResult()
        {
            // Arrange
            Website website = new Website();
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(website);

            var handler = new DeleteWebsiteHandler(_repositoryMock.Object, _unitOfWorkMock.Object);

            // Act
            var command = new Application.Websites.Commands.DeleteWebsite.DeleteWebsite(Guid.Empty);
            OperationResult<bool> operationResult = await handler.Handle(command, CancellationToken.None);

            // Assert
            operationResult.Should().BeOfType(typeof(OperationResult<bool>));
            operationResult.IsSuccessful.Should().BeTrue();
            operationResult.Errors.Should().BeNull();

            _unitOfWorkMock.Verify(x => x.CommitAsync(CancellationToken.None), Times.Once());
        }
    }
}
