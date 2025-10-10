using MediatR;
using SolarLab.AdvertBoard.Application.Abstractions;
using SolarLab.AdvertBoard.Domain.Adverts;
using SolarLab.AdvertBoard.Domain.Adverts.Events;
using SolarLab.AdvertBoard.Domain.Comments;

namespace SolarLab.AdvertBoard.Application.Adverts.DeletePublished
{
    public class DeleteCommentsOnPublishedAdvertDeletedDomainEventHandler(
        ICommentRepository commentRepository, 
        IUnitOfWork unitOfWork) : INotificationHandler<PublishedAdvertDeletedDomainEvent>
    {
        public async Task Handle(PublishedAdvertDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            await commentRepository.DeleteAllForAdvert(new AdvertId(notification.Id));

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
