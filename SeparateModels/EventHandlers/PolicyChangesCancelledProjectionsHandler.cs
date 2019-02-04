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
        private readonly PolicyVersionDtoProjection policyVersionDtoProjection;

        public PolicyChangesCancelledProjectionsHandler(PolicyInfoDtoProjection policyInfoDtoProjection, PolicyVersionDtoProjection policyVersionDtoProjection)
        {
            this.policyInfoDtoProjection = policyInfoDtoProjection;
            this.policyVersionDtoProjection = policyVersionDtoProjection;
        }

        public Task Handle(PolicyAnnexCancelled @event, CancellationToken cancellationToken)
        {
            policyInfoDtoProjection.UpdatePolicyInfoDto(@event.Policy, @event.CurrentVersionAfterAnnexCancellation);
            
            policyVersionDtoProjection.UpdatePolicyVersionDtoProjection(@event.CancelledAnnexVersion);
            
            return Task.CompletedTask;
        }
    }
}