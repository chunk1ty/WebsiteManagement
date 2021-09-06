using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Common;

namespace WebsiteManagement.Api.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult Errors(Dictionary<string, string> errors)
        {
            foreach (var error in errors)
            {
                if (error.Value is ErrorMessages.WebsiteNotFound)
                {
                    return NotFound();
                }

                ModelState.AddModelError(error.Key, error.Value);
            }

            return ValidationProblem(ModelState);
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
