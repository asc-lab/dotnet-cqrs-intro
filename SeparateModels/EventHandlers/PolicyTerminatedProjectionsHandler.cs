using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.ReadModels;

namespace SeparateModels.EventHandlers
{
    public class PolicyTerminatedProjectionsHandler : INotificationHandler<PolicyTerminated>
    {
        private readonly PolicyInfoDtoProjection policyInfoDtoProjection;
        private readonly PolicyVersionDtoProjection policyVersionDtoProjection;

        public PolicyTerminatedProjectionsHandler(PolicyInfoDtoProjection policyInfoDtoProjection, PolicyVersionDtoProjection policyVersionDtoProjection)
        {
            this.policyInfoDtoProjection = policyInfoDtoProjection;
            this.policyVersionDtoProjection = policyVersionDtoProjection;
        }

        public Task Handle(PolicyTerminated @event, CancellationToken cancellationToken)
        {
            policyInfoDtoProjection.UpdatePolicyInfoDto(@event.TerminatedPolicy, @event.TerminatedVersion);
            
            policyVersionDtoProjection.CreatePolicyVersionDtoProjection(@event.TerminatedPolicy, @event.TerminatedVersion);
            
            return Task.CompletedTask;
        }
    }
}