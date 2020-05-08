using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using WebsiteManagement.Application.Websites.Commands.CreateWebsite;

namespace WebsiteManagement.Application.UnitTests.Websites.Commands.CreateWebsite
{
    [TestFixture]
    public class CreateWebsiteValidatorTests
    {
        [Test]
        public void IsValid_WhenNameIsNull_ReturnFailureResult()
        {
            // Arrange
            var validator = new CreateWebsiteValidator();

            // Act
            var command = new Application.Websites.Commands.CreateWebsite.CreateWebsite(null, "www.mysite.com", new List<string> { "cat1,cat2" }, "myImage.png", "image/png", new byte[1], "ank@ank.bg", "123456");

            var validationResult = validator.IsValid(command);

            // Assert
            validationResult.IsSuccessful.Should().BeFalse();
            validationResult.ErrorMessage.Should().Be("Name cannot be empty.");
        }

        [Test]
        public void IsValid_WhenNameIsLessThan3Characters_ReturnFailureResult()
        {
            // Arrange
            var validator = new CreateWebsiteValidator();

            // Act
            var command = new Application.Websites.Commands.CreateWebsite.CreateWebsite("a", "www.mysite.com", new List<string> { "cat1,cat2" }, "myImage.png", "image/png", new byte[1], "ank@ank.bg", "123456");

            var validationResult = validator.IsValid(command);

            // Assert
            validationResult.IsSuccessful.Should().BeFalse();
            validationResult.ErrorMessage.Should().Be("Name should be more than 3 characters.");
        }

        [Test]
        public void IsValid_WhenNameIsMoreThan100Characters_ReturnFailureResult()
        {
            // Arrange
            var validator = new CreateWebsiteValidator();

            // Act
            var command = new Application.Websites.Commands.CreateWebsite.CreateWebsite("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "www.mysite.com", new List<string> { "cat1,cat2" }, "myImage.png", "image/png", new byte[1], "ank@ank.bg", "123456");

            var validationResult = validator.IsValid(command);

            // Assert
            validationResult.IsSuccessful.Should().BeFalse();
            validationResult.ErrorMessage.Should().Be("Name should be less than 100 characters.");
        }

        [TestCase(".csv")]
        [TestCase(".docx")]
        [TestCase(".ppt")]
        [TestCase(".bmp")]
        [TestCase(".gif")]
        public void IsValid_WhenFileExpensionNotSupported_ReturnFailureResult(string extenstion)
        {
            // Arrange
            var validator = new CreateWebsiteValidator();

            // Act
            var command = new Application.Websites.Commands.CreateWebsite.CreateWebsite("aaaa", "www.mysite.com", new List<string> { "cat1,cat2" }, $"myImage.{extenstion}", extenstion, new byte[1], "ank@ank.bg", "123456");

            var validationResult = validator.IsValid(command);

            // Assert
            validationResult.IsSuccessful.Should().BeFalse();
            validationResult.ErrorMessage.Should().Be("File extension is not supported. Supported types are: .jpg, .png, .jpeg");
        }

        [Test]
        public void IsValid_WhenFileIsGreaterThan5Mb_ReturnFailureResult()
        {
            // Arrange
            long fileLength = 6 * 1024 * 1024;

            var validator = new CreateWebsiteValidator();

            // Act
            var command = new Application.Websites.Commands.CreateWebsite.CreateWebsite("aaaa", "www.mysite.com", new List<string> { "cat1,cat2" }, "myImage.png", "image/png", new byte[fileLength], "ank@ank.bg", "123456");

            var validationResult = validator.IsValid(command);

            // Assert
            validationResult.IsSuccessful.Should().BeFalse();
            validationResult.ErrorMessage.Should().Be("File size is greater than 5 MB.");
        }


        [TestCase(".jpg")]
        [TestCase(".png")]
        [TestCase(".jpeg")]
        public void IsValid_WhenRequestIsValid_ReturnSuccessResult(string extenstion)
        {
            // Arrange
            var validator = new CreateWebsiteValidator();

            // Act
            var command = new Application.Websites.Commands.CreateWebsite.CreateWebsite("aaaa", "www.mysite.com", new List<string> { "cat1,cat2" }, $"myImage.{extenstion}", extenstion, new byte[1], "ank@ank.bg", "123456");

            var validationResult = validator.IsValid(command);

            // Assert
            validationResult.IsSuccessful.Should().BeTrue();
        }
        // Add more tests ..
    }
}
