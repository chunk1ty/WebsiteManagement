using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Images.Queries.GetImage;
using WebsiteManagement.Common;

namespace WebsiteManagement.Api.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/websites/{websiteId}/image")]
    public class ImageController : BaseApiController
    {
        [HttpGet(Name = "GetImageRequest")]
        public async Task<ActionResult> GetImage(Guid websiteId)
        {
            OperationResult<GetImageResponse> getImageOperation = await Mediator.Send(new GetImageRequest(websiteId));
            if (getImageOperation.IsSuccessful)
            {
                return File(getImageOperation.Result.Blob, getImageOperation.Result.ContentType, getImageOperation.Result.Name);
            }

            return Errors(getImageOperation.Errors);
        }
    }
}
