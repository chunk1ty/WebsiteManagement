using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebsiteManagement.Application.Identity.Queries.GetUser;
using WebsiteManagement.Common;

namespace WebsiteManagement.Api.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthenticationController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<LoginResponse>> Login([FromBody]LoginRequest model)
        {
            OperationResult<LoginResponse> authenticationOperation = await Mediator.Send(model);
            if (authenticationOperation.IsSuccessful)
            {
                return Ok(authenticationOperation.Result);
            }

            return Errors(authenticationOperation.Errors);
        }
    }
}
