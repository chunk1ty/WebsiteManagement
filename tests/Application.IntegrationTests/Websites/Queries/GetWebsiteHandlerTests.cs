using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Application.Websites;
using WebsiteManagement.Application.Websites.Queries.GetWebsite;
using WebsiteManagement.Domain;
using WebsiteManagement.Infrastructure.Persistence;

namespace WebsiteManagement.Application.IntegrationTests.Websites.Queries
{
    [TestFixture]
    public class GetWebsiteHandlerTests : BaseDatabaseFixture
    {
        [Test]
        public async Task HandleAsync_WithCorrectCommand_ShouldReturnWebsite()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            SeedWebsite(websiteId);

            IServiceScope scope = CreateScope();
            var handler = scope.ServiceProvider.GetService<IMediator<GetWebsite, WebsiteOutputModel>>();

            // Act
            var query = new GetWebsite(websiteId);
            var operationResult = await handler.HandleAsync(query);

            // Assert
            operationResult.IsSuccessful.Should().BeTrue();
            operationResult.ErrorMessage.Should().BeNull();
            operationResult.Should().BeOfType(typeof(OperationResult<WebsiteOutputModel>));

            WebsiteOutputModel actualWebsite = operationResult.Result;
            actualWebsite.Name.Should().Be("myWebsite");
            actualWebsite.Url.Should().Be("www.mysite.com");
            actualWebsite.Categories.Count.Should().Be(2);
            actualWebsite.Categories[0].Should().Be("category 1");
            actualWebsite.Categories[1].Should().Be("category 2");
            actualWebsite.Image.Name.Should().Be("myImage.png");
            actualWebsite.Login.Email.Should().Be("ank@ank.bg");
            actualWebsite.Login.Password.Should().Be("123456");
        }

        private void SeedWebsite(Guid websiteId)
        {
            IServiceScope scope = CreateScope();
            using (var db = scope.ServiceProvider.GetService<WebsiteManagementDbContext>())
            {
                var website = new Website()
                {
                    Id = websiteId,
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

                db.Websites.Add(website);
                db.SaveChanges();
            }
        }
    }
}
