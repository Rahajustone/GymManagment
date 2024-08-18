using GymManagement.Domain.Gyms;
using GymManagement.Domain.Subscriptions;
using TestCommon.TestConstants;

namespace TestCommon.Subscriptions;

public static class GymFactory 
{
    public static Gym CreateGym(
        string name = Constants.Gym.Name,
        int maxRooms = Constants.Subscriptions.MaxRoomsFreeTier,
        Guid? id = null)
    {
        return new Gym(
            name,
            maxRooms,
            subscriptionId: Constants.Subscriptions.Id,
            id: id ?? Constants.Subscriptions.Id
        );
    }
} 