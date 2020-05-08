using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.Websites.Queries.GetWebsite
{
    public sealed class GetWebsite : IRequest<OperationResult<WebsiteOutputModel>>
    {
        public GetWebsite(Guid websiteId)
        {
            WebsiteId = websiteId;
        }

        public Guid WebsiteId { get; }
    }

    public class GetWebsiteHandler : IRequestHandler<GetWebsite, OperationResult<WebsiteOutputModel>>
    {
        private readonly IWebsiteRepository _repository;
        private readonly ICypher _cypher;

        public GetWebsiteHandler(IWebsiteRepository repository, ICypher cypher)
        {
            _repository = repository;
            _cypher = cypher;
        }

        public async Task<OperationResult<WebsiteOutputModel>> Handle(GetWebsite query, CancellationToken cancellationToken)
        {
            query = query ?? throw new ArgumentNullException(nameof(query));

            Website website = await _repository.GetByIdAsync(query.WebsiteId);
            if (website is null)
            {
                return OperationResult<WebsiteOutputModel>.Failure(ErrorMessages.WebsiteNotFound);
            }

            return OperationResult<WebsiteOutputModel>.Success(website.ToWebsiteOutputModel(_cypher.Decrypt(website.Password)));
        }
    }
}
