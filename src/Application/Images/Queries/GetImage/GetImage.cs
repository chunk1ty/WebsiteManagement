using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.Images.Queries.GetImage
{
    public class GetImage : IRequest<OperationResult<ImageContentOutputModel>>
    {
        public GetImage(Guid websiteId)
        {
            WebsiteId = websiteId;
        }

        public Guid WebsiteId { get; }
    }

    public class GetImageHandler : IRequestHandler<GetImage, OperationResult<ImageContentOutputModel>>
    {
        private readonly IWebsiteRepository _repository;

        public GetImageHandler(IWebsiteRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<ImageContentOutputModel>> Handle(GetImage query, CancellationToken cancellationToken)
        {
            query = query ?? throw new ArgumentNullException(nameof(query));

            Website website = await _repository.GetByIdAsync(query.WebsiteId);
            if (website is null)
            {
                return OperationResult<ImageContentOutputModel>.Failure(ErrorMessages.WebsiteNotFound);
            }

            return OperationResult<ImageContentOutputModel>.Success(new ImageContentOutputModel(website.Image.Name, website.Image.MimeType, website.Image.Blob));
        }
    }
}
