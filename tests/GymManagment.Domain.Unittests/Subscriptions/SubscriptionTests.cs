using ErrorOr;
using FluentAssertions;
using GymManagement.Domain.Subscriptions;
using TestCommon.Subscriptions;

namespace GymManagment.Domain.Unittests.Subscription;

public class SubscriptionTests
{
    [Fact]
    public void AddGym_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // Arrange
        // Create a subscription
        var subscriptions = SubscriptionFactory.CreateSubscription();

        // Create the maximum number of gyms + 1
        var gyms = Enumerable.Range(0, subscriptions.GetMaxGyms() + 1)
            .Select( _ => GymFactory.CreateGym(id: Guid.NewGuid()))
            .ToList();  
        
        // Act
        var resutls = gyms.ConvertAll(gym => subscriptions.AddGym(gym));

        // Assert
        var allButLastGym = resutls[..^1];
        allButLastGym.Should().AllSatisfy(resutls => resutls.Value.Should().Be(Result.Success));

        var lastGymResult = resutls.Last();
        lastGymResult.IsError.Should().BeTrue();
        lastGymResult.FirstError.Should().Be(
            SubscriptionErrors.CannotHaveMoreGymsThanTheSubscriptionAllows);
    }
}  