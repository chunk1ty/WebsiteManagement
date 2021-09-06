using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebsiteManagement.Api.Models.Input;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Websites;
using WebsiteManagement.Application.Websites.Commands.DeleteWebsite;
using WebsiteManagement.Application.Websites.Queries.GetWebsite;
using WebsiteManagement.Application.Websites.Queries.GetWebsites;

namespace WebsiteManagement.Api.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/websites")]
    public class WebsiteController : BaseApiController
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

            return Errors(getWebsitesOperation.Errors);
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

            return Errors(getWebsiteOperation.Errors);
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

            return Errors(createWebsiteOperation.Errors);
        }

        [HttpPut("{websiteId}")]
        public async Task<ActionResult> UpdateWebsite([FromRoute]Guid websiteId, [FromForm]UpdateWebsiteInputModel updateWebsite)
        {
            OperationResult<bool> updateWebsiteOperation = await Mediator.Send(updateWebsite.ToUpdateWebsite(websiteId));
            if (updateWebsiteOperation.IsSuccessful)
            {
                return NoContent();
            }

            return Errors(updateWebsiteOperation.Errors);
        }

        [HttpDelete("{websiteId}")]
        public async Task<ActionResult> DeleteWebsite([FromRoute]Guid websiteId)
        {
            OperationResult<bool> deleteWebsiteOperation = await Mediator.Send(new DeleteWebsite(websiteId));
            if (deleteWebsiteOperation.IsSuccessful)
            {
                return NoContent();
            }

            return Errors(deleteWebsiteOperation.Errors);
        }

        [HttpOptions]
        public ActionResult GetWebsiteOptions()
        {
            Response.Headers.Add("Allow", "GET, HEAD, OPTIONS, POST, PUT, DELETE");
            return Ok();
        }
    }
}