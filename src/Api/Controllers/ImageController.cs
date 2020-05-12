using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Images.Queries.GetImage;

namespace WebsiteManagement.Api.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/websites/{websiteId}/image")]
    public class ImageController : ApiController
    {
        [HttpGet(Name = "GetImage")]
        public async Task<ActionResult> GetImage(Guid websiteId)
        {
            OperationResult<ImageContentOutputModel> getImageOperation = await Mediator.Send(new GetImage(websiteId));
            if (getImageOperation.IsSuccessful)
            {
                return File(getImageOperation.Result.Blob, getImageOperation.Result.ContentType, getImageOperation.Result.Name);
            }

            return Errors(getImageOperation.Errors);
        }
    }
}
