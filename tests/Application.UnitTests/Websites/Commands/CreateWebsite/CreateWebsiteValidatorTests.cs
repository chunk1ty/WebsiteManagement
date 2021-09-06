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
using WebsiteManagement.Application.Websites.Commands.Abstract;
using WebsiteManagement.Application.Websites.Commands.CreateWebsite;
using WebsiteManagement.Common;

namespace WebsiteManagement.Application.UnitTests.Websites.Commands.CreateWebsite
{
    [TestFixture]
    public class CreateWebsiteValidatorTests
    {
        private ValidationBehavior<Application.Websites.Commands.CreateWebsite.CreateWebsite, OperationResult<GetWebsiteResponse>> _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ValidationBehavior<Application.Websites.Commands.CreateWebsite.CreateWebsite, OperationResult<GetWebsiteResponse>>(new CreateWebsiteValidator());
        }

        [Test]
        public async Task IsValid_WhenNameIsNull_ReturnFailureResult()
        {
            // Act
            var command = new Application.Websites.Commands.CreateWebsite.CreateWebsite(null, "www.mysite.com", new List<string> { "cat1,cat2" }, new ImageManipulation("myImage.png", "image/png", new byte[1]), "ank@ank.bg", "123456");

            var validationResult = await _validator.Handle(command, CancellationToken.None, null);

            // Assert
            validationResult.IsSuccessful.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors.First().Key.Should().Be("Name");
            validationResult.Errors.First().Value.Should().Be("Name is required.");
        }

        [TestCase("a")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public async Task IsValid_WhenNameIsLessThan3Characters_ReturnFailureResult(string name)
        {
            // Act
            var command = new Application.Websites.Commands.CreateWebsite.CreateWebsite(name, "www.mysite.com", new List<string> { "cat1,cat2" }, new ImageManipulation("myImage.png", "image/png", new byte[1]), "ank@ank.bg", "123456");

            var validationResult = await _validator.Handle(command, CancellationToken.None, null);

            // Assert
            validationResult.IsSuccessful.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors.First().Key.Should().Be("Name");
            validationResult.Errors.First().Value.Should().Be("Name should be more than 3 and less than 100 characters.");
        }

        [TestCase(".csv")]
        [TestCase(".docx")]
        [TestCase(".ppt")]
        [TestCase(".bmp")]
        [TestCase(".gif")]
        public async Task IsValid_WhenFileExpensionNotSupported_ReturnFailureResult(string extenstion)
        {
            // Act
            var command = new Application.Websites.Commands.CreateWebsite.CreateWebsite("aaaa", "www.mysite.com", new List<string> { "cat1,cat2" }, new ImageManipulation($"$myImage{extenstion}", "image/png", new byte[1]), "ank@ank.bg", "123456");

            var validationResult = await _validator.Handle(command, CancellationToken.None, null);

            // Assert
            validationResult.IsSuccessful.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors.First().Key.Should().Be("Image");
            validationResult.Errors.First().Value.Should().Be("File extension is not supported. Supported types   are: .jpg, .png, .jpeg");
        }

        [Test]
        public async Task IsValid_WhenFileIsGreaterThan5Mb_ReturnFailureResult()
        {
            long fileLength = 6 * 1024 * 1024;

            // Act
            var command = new Application.Websites.Commands.CreateWebsite.CreateWebsite("aaaa", "www.mysite.com", new List<string> { "cat1,cat2" }, new ImageManipulation("$myImage.png", "image/png", new byte[fileLength]), "ank@ank.bg", "123456");

            var validationResult = await _validator.Handle(command, CancellationToken.None, null);

            // Assert
            validationResult.IsSuccessful.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors.First().Key.Should().Be("Image");
            validationResult.Errors.First().Value.Should().Be("File size is greater than 5 MB.");
        }

        [Test]
        public async Task IsValid_WhenRequestIsValid_ReturnSuccessResult()
        {
            // Act
            var command = new Application.Websites.Commands.CreateWebsite.CreateWebsite("aaaa", "www.mysite.com", new List<string> { "cat1,cat2" }, new ImageManipulation("$myImage.png", "image/png", new byte[1]), "ank@ank.bg", "123456");

            RequestHandlerDelegate<OperationResult<GetWebsiteResponse>> requestHandlerDelegate = () => Task.FromResult(OperationResult<GetWebsiteResponse>.Success(null));

            var validationResult = await _validator.Handle(command, CancellationToken.None, requestHandlerDelegate);

            // Assert
            validationResult.IsSuccessful.Should().BeTrue();
        }

        //// Add more tests ..
    }
}
