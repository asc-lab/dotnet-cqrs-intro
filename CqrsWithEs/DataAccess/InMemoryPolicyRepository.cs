using System;
using CqrsWithEs.Domain.Policy;


namespace CqrsWithEs.DataAccess
{
    public class InMemoryPolicyRepository : IPolicyRepository
    {
        private readonly IEventStore eventStore;

        public InMemoryPolicyRepository(IEventStore eventStore)
        {
            this.eventStore = eventStore;
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