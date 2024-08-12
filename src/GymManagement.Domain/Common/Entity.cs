namespace GymManagement.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; set; }

    protected readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(Guid id) => Id = id;
    protected Entity()
    {
    }

    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToList();

        _domainEvents.Clear();

        return copy;
    }
}