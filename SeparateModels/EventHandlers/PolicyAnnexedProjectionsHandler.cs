using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.ReadModels;

namespace SeparateModels.EventHandlers
{
    public class PolicyAnnexedProjectionsHandler :
        INotificationHandler<PolicyAnnexed>
    {
        private readonly PolicyInfoDtoProjection policyInfoDtoProjection;

        public PolicyAnnexedProjectionsHandler(PolicyInfoDtoProjection policyInfoDtoProjection)
        {
            this.policyInfoDtoProjection = policyInfoDtoProjection;
        }

        public Task Handle(PolicyAnnexed @event, CancellationToken cancellationToken)
        {
            policyInfoDtoProjection.UpdatePolicyInfoDto(@event.AnnexedPolicy, @event.AnnexVersion);
            
            return Task.CompletedTask;
        }
    }
}