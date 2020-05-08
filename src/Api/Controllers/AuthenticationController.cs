using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Identity.Queries.GetUser;

namespace WebsiteManagement.Api.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthenticationController : ApiController
    {
        [HttpGet]
        
        public async Task<ActionResult<UserOutputModel>> Login([FromBody]GetUser model)
        {
            OperationResult<UserOutputModel> authenticationOperation = await Mediator.Send(model);
            if (authenticationOperation.IsSuccessful)
            {
                return Ok(authenticationOperation.Result);
            }

            ModelState.AddModelError(string.Empty, authenticationOperation.ErrorMessage);
            return ValidationProblem(ModelState);
        }
    }
}
