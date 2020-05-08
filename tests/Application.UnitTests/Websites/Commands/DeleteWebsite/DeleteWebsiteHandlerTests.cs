using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Application.Websites.Commands.DeleteWebsite;
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
        public void HandleAsync_WhenCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var handler = new DeleteWebsiteHandler(_repositoryMock.Object, _unitOfWorkMock.Object);

            // Act
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => handler.HandleAsync(null));

            // Assert
            ex.Message.Should().Contain("command");
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
            OperationResult<bool> operationResult = await handler.HandleAsync(command);

            // Assert
            operationResult.Should().BeOfType(typeof(OperationResult<bool>));
            operationResult.IsSuccessful.Should().BeFalse();
            operationResult.ErrorMessage.Should().Be(ErrorMessages.WebsiteNotFound);
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
            OperationResult<bool> operationResult = await handler.HandleAsync(command);

            // Assert
            operationResult.Should().BeOfType(typeof(OperationResult<bool>));
            operationResult.IsSuccessful.Should().BeTrue();
            operationResult.ErrorMessage.Should().BeNull();

            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once());
        }
    }
}
