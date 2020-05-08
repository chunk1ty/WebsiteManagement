using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebsiteManagement.Api.Models.Input;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Websites;
using WebsiteManagement.Application.Websites.Commands.DeleteWebsite;
using WebsiteManagement.Application.Websites.Queries.GetWebsite;
using WebsiteManagement.Application.Websites.Queries.GetWebsites;

namespace WebsiteManagement.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/websites")]
    public class WebsiteController : ApiController
    {
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<List<WebsiteOutputModel>>> GetWebsites([FromQuery] GetWebsites getWebsites)
        {
            OperationResult<List<WebsiteOutputModel>> getWebsitesOperation = await Mediator.Send(getWebsites);
            if (getWebsitesOperation.IsSuccessful)
            {
                getWebsitesOperation.Result.ForEach(x => x.Image.DownloadLink = Url.Link("GetImage", new { websiteId = x.Id }));

                return Ok(getWebsitesOperation.Result);
            }

            ModelState.AddModelError(string.Empty, getWebsitesOperation.ErrorMessage);
            return ValidationProblem(ModelState);
        }

        [HttpGet("{websiteId}", Name = "GetWebsite")]
        public async Task<ActionResult<WebsiteOutputModel>> GetWebsite([FromRoute]Guid websiteId)
        {
            OperationResult<WebsiteOutputModel> getWebsiteOperation = await Mediator.Send(new GetWebsite(websiteId));
            if (getWebsiteOperation.IsSuccessful)
            {
                getWebsiteOperation.Result.Image.DownloadLink = Url.Link("GetImage", new { websiteId = getWebsiteOperation.Result.Id });
                return Ok(getWebsiteOperation.Result);
            }

            if (getWebsiteOperation.ErrorMessage is ErrorMessages.WebsiteNotFound)
            {
                return NotFound();
            }

            ModelState.AddModelError(string.Empty, getWebsiteOperation.ErrorMessage);
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public async Task<ActionResult<WebsiteOutputModel>> CreateWebsite([FromForm]CreateWebsiteInputModel createWebsiteInputModel)
        {
            OperationResult<WebsiteOutputModel> createWebsiteOperation = await Mediator.Send(createWebsiteInputModel.ToCreateWebsite());
            if (createWebsiteOperation.IsSuccessful)
            {
                createWebsiteOperation.Result.Image.DownloadLink = Url.Link("GetImage", new { websiteId = createWebsiteOperation.Result.Id });
                return CreatedAtRoute("GetWebsite", new { websiteId = createWebsiteOperation.Result.Id }, createWebsiteOperation.Result);
            }

            ModelState.AddModelError(string.Empty, createWebsiteOperation.ErrorMessage);
            return ValidationProblem(ModelState);
        }

        [HttpPut("{websiteId}")]
        public async Task<ActionResult> UpdateWebsite([FromRoute]Guid websiteId, [FromForm]UpdateWebsiteInputModel updateWebsite)
        {
            OperationResult<bool> updateWebsiteOperation = await Mediator.Send(updateWebsite.ToUpdateWebsite(websiteId));

            if (updateWebsiteOperation.IsSuccessful)
            {
                return NoContent();
            }

            if (updateWebsiteOperation.ErrorMessage is ErrorMessages.WebsiteNotFound)
            {
                return NotFound();
            }

            ModelState.AddModelError(string.Empty, updateWebsiteOperation.ErrorMessage);
            return ValidationProblem(ModelState);
        }

        [HttpDelete("{websiteId}")]
        public async Task<ActionResult> DeleteWebsite([FromRoute]Guid websiteId)
        {
            OperationResult<bool> deleteWebsiteOperation = await Mediator.Send(new DeleteWebsite(websiteId));
            if (deleteWebsiteOperation.IsSuccessful)
            {
                return NoContent();
            }

            if (deleteWebsiteOperation.ErrorMessage is ErrorMessages.WebsiteNotFound)
            {
                return NotFound();
            }

            ModelState.AddModelError(string.Empty, deleteWebsiteOperation.ErrorMessage);
            return ValidationProblem(ModelState);
        }

        [HttpOptions]
        public ActionResult GetWebsiteOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, OPTIONS, POST, PUT, DELETE");
            return Ok();
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}