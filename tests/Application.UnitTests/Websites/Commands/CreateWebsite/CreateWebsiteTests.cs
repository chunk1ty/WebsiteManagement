using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using WebsiteManagement.Application.Websites.Commands.Abstract;

namespace WebsiteManagement.Application.UnitTests.Websites.Commands.CreateWebsite
{
    [TestFixture]
    public class CreateWebsiteTests
    {
        [Test]
        public void ToWebsite_WithCorrectCreateWebsite_ShouldReturnWebsite()
        {
            // Arrange
            var createWebsite = new Application.Websites.Commands.CreateWebsite.CreateWebsite("mySite", "www.ank.com", new List<string> { "cat" }, new ImageManipulation("myimg", "png", new byte[4]), "a@ank.bg", "123456");

            // Act
            var website = createWebsite.ToWebsite("encryptedPassword");

            // Assert
            website.Name.Should().Be("mySite");
            website.Url.Should().Be("www.ank.com");
            website.Categories.Count.Should().Be(1);
            website.Categories.First().Value.Should().Be("cat");
            website.Image.Name.Should().Be("myimg");
            website.Image.MimeType.Should().Be("png");
            website.Image.Blob.Length.Should().Be(4);
            website.Email.Should().Be("a@ank.bg");
            website.Password.Should().Be("encryptedPassword");
            website.IsDeleted.Should().Be(false);
        }
    }
}
