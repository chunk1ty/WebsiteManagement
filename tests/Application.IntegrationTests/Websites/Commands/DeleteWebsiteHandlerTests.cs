using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Application.Websites.Commands.DeleteWebsite;
using WebsiteManagement.Domain;
using WebsiteManagement.Infrastructure.Persistence;

namespace WebsiteManagement.Application.IntegrationTests.Websites.Commands
{
    [TestFixture]
    public class DeleteWebsiteHandlerTests : BaseDatabaseFixture
    {
        [Test]
        public async Task HandleAsync_WithCorrectCommand_ShouldUpdateWebsite()
        {
            // Arrange
            var websiteId = Guid.NewGuid();
            SeedWebsite(websiteId);

            IServiceScope scope = CreateScope();
            var handler = scope.ServiceProvider.GetService<IMediator<DeleteWebsite, bool>>();

            // Act
            var command = new DeleteWebsite(websiteId);
            var operationResult = await handler.HandleAsync(command);

            // Assert
            operationResult.IsSuccessful.Should().BeTrue();
            operationResult.ErrorMessage.Should().BeNull();
            operationResult.Should().BeOfType(typeof(OperationResult<bool>));

            Website actualWebsite;
            using (var db = scope.ServiceProvider.GetService<WebsiteManagementDbContext>())
            {
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                actualWebsite = db.Websites.Include(x => x.Image)
                                            .Include(x => x.Categories)
                                            .SingleOrDefault(x => x.Id == websiteId);
            }

            actualWebsite.IsDeleted.Should().BeTrue();
        }

        private void SeedWebsite(Guid websiteId)
        {
            IServiceScope scope = CreateScope();
            using (var db = scope.ServiceProvider.GetService<WebsiteManagementDbContext>())
            {
                var website = new Website
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
                    Categories = new List<Category> { new Category() { Value = "category 1" }, new Category() { Value = "category 2" } }
                };

                db.Websites.Add(website);
                db.SaveChanges();
            }
        }
    }
}