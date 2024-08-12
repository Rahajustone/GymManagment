using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscription;

public class DeleteSubscriptionCommandHandler(
    IAdminsRepository adminsRepository,
    ISubscriptionsRepository subscriptionsRepository,
    IUnitOfWork unitOfWork)
        : IRequestHandler<DeleteSubscriptionCommand, ErrorOr<Deleted>>
{
    private readonly IAdminsRepository _adminsRepository = adminsRepository;
    private readonly ISubscriptionsRepository _subscriptionsRepository = subscriptionsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ErrorOr<Deleted>> Handle(DeleteSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionsRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription is null)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        var admin = await _adminsRepository.GetByIdAsync(subscription.AdminId);

        if (admin is null)
        {
            return Error.Unexpected(description: "Admin not found");
        }

        admin.DeleteSubscription(command.SubscriptionId);

        // Use Domain Event Pattern
        // var gymsToDelete = await _gymsRepository.ListBySubscriptionIdAsync(command.SubscriptionId);

        await _adminsRepository.UpdateAsync(admin);

        // Use Domain Event Pattern
        // await _subscriptionsRepository.RemoveSubscriptionAsync(subscription);
        // await _gymsRepository.RemoveRangeAsync(gymsToDelete);
        
        await _unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}
