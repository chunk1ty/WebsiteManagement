using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Common;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.Websites.Queries.GetWebsite
{
    public sealed class GetWebsiteRequest : IRequest<OperationResult<GetWebsiteResponse>>
    {
        public GetWebsiteRequest(Guid websiteId)
        {
            WebsiteId = websiteId;
        }

        public Guid WebsiteId { get; }
    }

    public class GetWebsiteHandler : IRequestHandler<GetWebsiteRequest, OperationResult<GetWebsiteResponse>>
    {
        private readonly IWebsiteRepository _repository;
        private readonly ICypher _cypher;

        public GetWebsiteHandler(IWebsiteRepository repository, ICypher cypher)
        {
            _repository = repository;
            _cypher = cypher;
        }

        public async Task<OperationResult<GetWebsiteResponse>> Handle(GetWebsiteRequest request, CancellationToken cancellationToken)
        {
            Website website = await _repository.GetByIdAsync(request.WebsiteId);
            if (website is null)
            {
                return OperationResult<GetWebsiteResponse>.Failure(new Dictionary<string, string> { { "WebsiteId", ErrorMessages.WebsiteNotFound } });
            }

            return OperationResult<GetWebsiteResponse>.Success(website.ToWebsiteOutputModel(_cypher.Decrypt(website.Password)));
        }
    }
}
