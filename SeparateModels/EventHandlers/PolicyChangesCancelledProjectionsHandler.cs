using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.ReadModels;

namespace SeparateModels.EventHandlers
{
    public class PolicyChangesCancelledProjectionsHandler :
        INotificationHandler<PolicyAnnexCancelled>
    {
        private readonly PolicyInfoDtoProjection policyInfoDtoProjection;

        public PolicyChangesCancelledProjectionsHandler(PolicyInfoDtoProjection policyInfoDtoProjection)
        {
            this.policyInfoDtoProjection = policyInfoDtoProjection;
        }

        public Task Handle(PolicyAnnexCancelled @event, CancellationToken cancellationToken)
        {
            policyInfoDtoProjection.UpdatePolicyInfoDto(@event.Policy, @event.CurrentVersionAfterAnnexCancellation);
            
            return Task.CompletedTask;
        }
    }
}