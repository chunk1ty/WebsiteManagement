using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Application.Websites.Commands.Abstract;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.Websites.Commands.UpdateWebsite
{
    public sealed class UpdateWebsite : WebsiteManipulation, IRequest<OperationResult<bool>>
    {
        public UpdateWebsite(Guid websiteId, 
                             string name, 
                             string url, 
                             List<string> categories,
                             ImageManipulation image,
                             string email, 
                             string password) 
            : base(name, url, categories, image, email, password)
        {
            WebsiteId = websiteId;
        }

        public Guid WebsiteId { get; }
    }

    public class UpdateWebsiteHandler : IRequestHandler<UpdateWebsite, OperationResult<bool>>
    {
        private readonly IWebsiteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICypher _cypher;

        public UpdateWebsiteHandler(IWebsiteRepository repository,
            IUnitOfWork unitOfWork,
            ICypher cypher)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _cypher = cypher;
        }

        public async Task<OperationResult<bool>> Handle(UpdateWebsite request, CancellationToken cancellationToken)
        {
            Website website = await _repository.GetByIdAsync(request.WebsiteId);
            if (website is null)
            {
                return OperationResult<bool>.Failure(new Dictionary<string, string> { { "WebsiteId", ErrorMessages.WebsiteNotFound } });
            }

            website.Update(request.ToWebsite(_cypher.Encrypt(request.Password)));

            try
            {
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (UrlExistsException)
            {
                return OperationResult<bool>.Failure(new Dictionary<string, string> { { "Url", "Url already exists." } });
            }

            return OperationResult<bool>.Success(true);
        }
    }
}
