using System;
using CqrsWithEs.Domain.Policy;
using MediatR;


namespace CqrsWithEs.DataAccess
{
    public class InMemoryPolicyRepository : IPolicyRepository
    {
        private readonly IEventStore eventStore;
        private readonly IMediator bus;

        public InMemoryPolicyRepository(IEventStore eventStore, IMediator bus)
        {
            this.eventStore = eventStore;
            this.bus = bus;
            this.eventStore.Bus = bus;
        }

        public Policy GetById(Guid Id)
        {
            var events = eventStore.GetEventsForAggregate(Id);
            return new Policy(Id, events);
        }

        public void Save(Policy policy, int expectedVersion)
        {
            eventStore.SaveEvents(policy.Id, policy.GetUncommittedChanges(), expectedVersion);
            policy.MarkChangesAsCommitted();
        }
    }
}