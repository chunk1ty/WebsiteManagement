using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Common;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.Websites.Queries.GetWebsites
{
    public sealed class GetWebsitesRequest : IRequest<OperationResult<List<GetWebsiteResponse>>>
    {
        private const int MaxPageSize = 20;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string OrderBy { get; set; }
    }

    public class GetWebsitesHandler : IRequestHandler<GetWebsitesRequest, OperationResult<List<GetWebsiteResponse>>>
    {
        private readonly IWebsiteRepository _repository;
        private readonly ICypher _cypher;

        public GetWebsitesHandler(IWebsiteRepository repository, ICypher cypher)
        {
            _repository = repository;
            _cypher = cypher;
        }

        public async Task<OperationResult<List<GetWebsiteResponse>>> Handle(GetWebsitesRequest request, CancellationToken cancellationToken)
        {
            List<Website> websites = await _repository.GetAll(request.PageNumber, request.PageSize, request.OrderBy);

            List<GetWebsiteResponse> websitesOutputModel = websites.Select(w => w.ToWebsiteOutputModel(_cypher.Decrypt(w.Password))).ToList();

            return OperationResult<List<GetWebsiteResponse>>.Success(websitesOutputModel);
        }
    }
}
