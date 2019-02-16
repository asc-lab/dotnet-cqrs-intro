using System.Collections;
using System.Collections.Generic;
using CqrsWithEs.Domain.Base;
using KellermanSoftware.CompareNetObjects;
using Xunit;

namespace CqrsWithEs.Tests.Asserts
{
    public class PolicyEventsStreamAssert
    {
        private readonly IEnumerable<Event> events;

        public PolicyEventsStreamAssert(IEnumerable<Event> events)
        {
            this.events = events;
        }

        public PolicyEventsStreamAssert BeSingle()
        {
            Assert.Single(events);
            return this;
        }

        public PolicyEventsStreamAssert ContainEvent<T>(T expectedEvent) where T : Event
        {
            var comparer = new CompareLogic {Config = {MembersToIgnore = new List<string> {"Id", "Version"}}};
            Assert.Contains(events, ev => comparer.Compare(ev, expectedEvent).AreEqual);
            return this;
        }
    }

    public static class PolicyEventsStreamAssertExtension
    {
        public static PolicyEventsStreamAssert Should(this IEnumerable<Event> events)
        {
            return new PolicyEventsStreamAssert(events);
        }
    }
}