using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Application.Websites.Commands.Abstract;
using WebsiteManagement.Common;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.Websites.Commands.CreateWebsite
{
    public class CreateWebsite : WebsiteManipulation, IRequest<OperationResult<GetWebsiteResponse>>
    {
        public CreateWebsite(string name,
                             string url,
                             List<string> categories,
                             ImageManipulation image,
                             string email,
                             string password)
            : base(name, url, categories, image, email, password)
        {
        }
    }

    public class CreateWebsiteHandler : IRequestHandler<CreateWebsite, OperationResult<GetWebsiteResponse>>
    {
        private readonly IWebsiteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICypher _cypher;

        public CreateWebsiteHandler(IWebsiteRepository repository, IUnitOfWork unitOfWork, ICypher cypher)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _cypher = cypher;
        }

        public async Task<OperationResult<GetWebsiteResponse>> Handle(CreateWebsite request, CancellationToken cancellationToken)
        {
            var passwordAsPlainText = request.Password;

            Website website = request.ToWebsite(_cypher.Encrypt(request.Password));

            _repository.Add(website);

            try
            {
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (UrlExistsException)
            {
                return OperationResult<GetWebsiteResponse>.Failure(new Dictionary<string, string> { { "Url", "Url already exists." } });
            }

            return OperationResult<GetWebsiteResponse>.Success(website.ToWebsiteOutputModel(passwordAsPlainText));
        }
    }
}