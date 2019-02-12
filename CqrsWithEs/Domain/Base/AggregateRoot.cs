using System;
using System.Collections.Generic;
using CqrsWithEs.Dragons;
using MediatR;

namespace CqrsWithEs.Domain.Base
{
    public class AggregateRoot
    {
        private readonly List<Event> _changes = new List<Event>();

        public Guid Id { get; protected set; }
        public int Version { get; internal set; }

        public IEnumerable<Event> GetUncommittedChanges() {
            return _changes;
        }

        public void MarkChangesAsCommitted() {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<Event> history) {
            foreach (var e in history)
            {
                ApplyChange(e, false);
                Version += 1;
            }
        }

        protected void ApplyChange(Event @event) {
            ApplyChange(@event, true);
        }

        // push atomic aggregate changes to local history for further processing (EventStore.SaveEvents)
        private void ApplyChange(Event @event, bool isNew) {
            this.AsDynamic().Apply(@event);
            if (isNew) _changes.Add(@event);
        }
    }

    public interface Message { }
    public class Command : Message { }

    public class Event : Message, INotification
    {
        public Guid Id;
        public int Version;
        public Event() {
            Id = Guid.NewGuid();
        }
    }
}