using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Websites;
using WebsiteManagement.Application.Websites.Queries.GetWebsites;
using WebsiteManagement.Domain;
using WebsiteManagement.Infrastructure.Persistence;

namespace WebsiteManagement.Application.IntegrationTests.Websites.Queries
{
    [TestFixture]
    public class GetWebsitesHandlerTests : BaseDatabaseFixture
    {
        [Test]
        public async Task HandleAsync_WithCorrectCommand_ShouldUpdateWebsites()
        {
            // Arrange
            SeedWebsites();

            IServiceScope scope = CreateScope();
            var handler = scope.ServiceProvider.GetService<IRequestHandler<GetWebsites, OperationResult<List<WebsiteOutputModel>>>>();

            // Act
            var request = new GetWebsites { PageNumber = 1, PageSize = 3, OrderBy = string.Empty };
            var getWebsitesOperation = await handler.Handle(request, CancellationToken.None);

            // Assert
            getWebsitesOperation.Result.Count.Should().Be(2);
            getWebsitesOperation.IsSuccessful.Should().BeTrue();
            getWebsitesOperation.Errors.Should().BeNull();
            getWebsitesOperation.Should().BeOfType(typeof(OperationResult<List<WebsiteOutputModel>>));

            WebsiteOutputModel actualWebsite = getWebsitesOperation.Result.SingleOrDefault(w => w.Url == "www.mysite.com");
            actualWebsite.Name.Should().Be("myWebsite");
            actualWebsite.Url.Should().Be("www.mysite.com");
            actualWebsite.Categories.Count.Should().Be(2);
            actualWebsite.Categories[0].Should().Be("category 1");
            actualWebsite.Categories[1].Should().Be("category 2");
            actualWebsite.Image.Name.Should().Be("myImage.png");
            actualWebsite.Login.Email.Should().Be("ank@ank.bg");
            actualWebsite.Login.Password.Should().Be("123456");


            WebsiteOutputModel actualWebsite1 = getWebsitesOperation.Result.SingleOrDefault(w => w.Url == "www.mysite1.com");
            actualWebsite1.Name.Should().Be("myWebsite1");
            actualWebsite1.Url.Should().Be("www.mysite1.com");
            actualWebsite1.Categories.Count.Should().Be(2);
            actualWebsite1.Categories[0].Should().Be("category 11");
            actualWebsite1.Categories[1].Should().Be("category 21");
            actualWebsite1.Image.Name.Should().Be("myImage1.png");
            actualWebsite1.Login.Email.Should().Be("ank1@ank.bg");
            actualWebsite1.Login.Password.Should().Be("123456");
        }

        private void SeedWebsites()
        {
            IServiceScope scope = CreateScope();
            using (var db = scope.ServiceProvider.GetService<WebsiteManagementDbContext>())
            {
                var website = new Website()
                {
                    Id = Guid.NewGuid(),
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

                var website1 = new Website()
                {
                    Id = Guid.NewGuid(),
                    Name = "myWebsite1",
                    Url = "www.mysite1.com",
                    Password = "dTEknBrlH8Wy5+tYfm6teQ==",
                    Email = "ank1@ank.bg",
                    Image = new Image()
                    {
                        Name = "myImage1.png",
                        Blob = new byte[17],
                        MimeType = "image/png"
                    },
                    Categories = new List<Category> { new Category { Value = "category 11" }, new Category() { Value = "category 21" } }
                };
                db.Websites.Add(website1);

                var website2 = new Website()
                {
                    Id = Guid.NewGuid(),
                    Name = "myWebsite2",
                    Url = "www.mysite2.com",
                    Password = "dTEknBrlH8Wy5+tYfm6teQ==",
                    Email = "ank2@ank.bg",
                    Image = new Image()
                    {
                        Name = "myImage2.png",
                        Blob = new byte[17],
                        MimeType = "image/png"
                    },
                    Categories = new List<Category> { new Category { Value = "category 112" }, new Category() { Value = "category 212" } },
                    IsDeleted = true
                };
                db.Websites.Add(website2);

                db.SaveChanges();
            }
        }
    }
}
