using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;

namespace SeparateModels.EventHandlers
{
    public class PolicyCreatedProjectionsHandler : INotificationHandler<PolicyCreated>
    {
        public Task Handle(PolicyCreated @event, CancellationToken cancellationToken)
        {
            Console.WriteLine("Policy created " + @event.NewPolicy.Number);
            return Task.CompletedTask;
        }
    }
}