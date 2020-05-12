using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace WebsiteManagement.Application.Common.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);

            ValidationResult validationResult = _validator.Validate(context);
            if (validationResult.IsValid)
            {
                return next();
            }

            Dictionary<string, string> errors = validationResult.Errors.ToDictionary(key => key.PropertyName, value => value.ErrorMessage);

            dynamic result = typeof(TResponse).GetMethod("Failure")?.Invoke(null, new object[] { errors });

            return Task.FromResult<TResponse>(result);
        }
    }
}