using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Application.Websites.Commands.DeleteWebsite
{
    public sealed class DeleteWebsite : IRequest<OperationResult<bool>>
    {
        public DeleteWebsite(Guid websiteId)
        {
            WebsiteId = websiteId;
        }

        public Guid WebsiteId { get; }
    }

    public class DeleteWebsiteHandler : IRequestHandler<DeleteWebsite, OperationResult<bool>>
    {
        private readonly IWebsiteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWebsiteHandler(IWebsiteRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<bool>> Handle(DeleteWebsite command, CancellationToken cancellationToken)
        {
            command = command ?? throw new ArgumentNullException(nameof(command));

            Website website = await _repository.GetByIdAsync(command.WebsiteId);
            if (website is null)
            {
                return OperationResult<bool>.Failure(ErrorMessages.WebsiteNotFound);
            }

            website.IsDeleted = true;
            await _unitOfWork.CommitAsync(cancellationToken);

            return OperationResult<bool>.Success(true);
        }
    }
}
