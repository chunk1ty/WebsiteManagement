using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Websites;
using WebsiteManagement.Application.Websites.Commands.Abstract;
using WebsiteManagement.Application.Websites.Commands.CreateWebsite;
using WebsiteManagement.Domain;
using WebsiteManagement.Infrastructure.Persistence;

namespace WebsiteManagement.Application.IntegrationTests.Websites.Commands
{
    [TestFixture]
    public class CreateWebsiteHandlerTests : BaseDatabaseFixture
    {
        [Test]
        public async Task HandleAsync_WithCorrectCommand_ShouldCreateWebsite()
        {
            // Arrange
            IServiceScope scope = CreateScope();
            var handler = scope.ServiceProvider.GetService<IRequestHandler<CreateWebsite, OperationResult<WebsiteOutputModel>>>();

               // Act
               var request = new CreateWebsite("mySite", "www.mysite.com", new List<string> { "cat1", "cat2" }, new ImageManipulation("myImage.png", "image/png", new byte[17]),  "ank@ank.bg", "123456");
            var createWebsiteOperationResult = await handler.Handle(request, CancellationToken.None);

            // Assert
            createWebsiteOperationResult.IsSuccessful.Should().BeTrue();
            createWebsiteOperationResult.Errors.Should().BeNull();
            createWebsiteOperationResult.Should().BeOfType(typeof(OperationResult<WebsiteOutputModel>));

            Website actualWebsite;
            using (var db = scope.ServiceProvider.GetService<WebsiteManagementDbContext>())
            {
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                actualWebsite = db.Websites.Include(x => x.Image)
                                            .Include(x => x.Categories)
                                            .SingleOrDefault(x => x.Id == createWebsiteOperationResult.Result.Id);
            }

            actualWebsite.Name.Should().Be("mySite");
            actualWebsite.Url.Should().Be("www.mysite.com");
            actualWebsite.Email.Should().Be("ank@ank.bg");
            actualWebsite.Password.Should().Be("dTEknBrlH8Wy5+tYfm6teQ==");
            actualWebsite.Image.Name.Should().Be("myImage.png");
            actualWebsite.Image.MimeType.Should().Be("image/png");
            actualWebsite.Image.Blob.Length.Should().Be(17);
            actualWebsite.Categories[0].Value.Should().Be("cat1");
            actualWebsite.Categories[1].Value.Should().Be("cat2");
        }
    }
}