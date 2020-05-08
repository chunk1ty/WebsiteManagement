using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebsiteManagement.Application.Interfaces;

namespace WebsiteManagement.Application.Common.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            request = request ?? throw new ArgumentNullException(nameof(request));

            if (!_validators.Any())
            {
                return next();
            }

            foreach (var validator in _validators)
            {
                OperationResult<bool> operationResult = validator.IsValid(request);

                if (!operationResult.IsSuccessful)
                {
                    dynamic result = typeof(TResponse).GetMethod("Failure").Invoke(null, new[] { operationResult.ErrorMessage });

                    return Task.FromResult<TResponse>(result);
                }
            }

            return next();
        }
    }
}