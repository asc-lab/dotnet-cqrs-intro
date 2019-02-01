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

        public PolicyTerminatedProjectionsHandler(PolicyInfoDtoProjection policyInfoDtoProjection)
        {
            this.policyInfoDtoProjection = policyInfoDtoProjection;
        }

        public Task Handle(PolicyTerminated @event, CancellationToken cancellationToken)
        {
            policyInfoDtoProjection.UpdatePolicyInfoDto(@event.TerminatedPolicy, @event.TerminatedVersion);
            
            return Task.CompletedTask;
        }
    }
}