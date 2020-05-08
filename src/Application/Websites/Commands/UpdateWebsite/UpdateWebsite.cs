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
                             string imageName, 
                             string imageContentType, 
                             byte[] imageBlob, 
                             string email, 
                             string password) 
            : base(name, url, categories, imageName, imageContentType, imageBlob, email, password)
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

        public async Task<OperationResult<bool>> Handle(UpdateWebsite command, CancellationToken cancellationToken)
        {
            Website website = await _repository.GetByIdAsync(command.WebsiteId);
            if (website is null)
            {
                return OperationResult<bool>.Failure(ErrorMessages.WebsiteNotFound);
            }

            website.Update(command.ToWebsite(_cypher.Encrypt(command.Password)));

            try
            {
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (UrlExistsException)
            {
                return OperationResult<bool>.Failure("Url already exists.");
            }

            return OperationResult<bool>.Success(true);
        }
    }
}
