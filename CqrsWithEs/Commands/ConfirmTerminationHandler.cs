using System.Threading;
using System.Threading.Tasks;
using CqrsWithEs.Domain.Policy;
using MediatR;

namespace CqrsWithEs.Commands
{
    public class ConfirmTerminationHandler : IRequestHandler<ConfirmTerminationCommand, ConfirmTerminationResult>
    {
        private readonly IPolicyRepository policyRepository;

        public ConfirmTerminationHandler(IPolicyRepository policyRepository)
        {
            this.policyRepository = policyRepository;
        }

        public Task<ConfirmTerminationResult> Handle(ConfirmTerminationCommand request, CancellationToken cancellationToken)
        {
            var policy = policyRepository.GetById(request.PolicyId);
            
            policy.ConfirmTermination();
            
            policyRepository.Save(policy, policy.Version);
            
            return Task.FromResult(new ConfirmTerminationResult
            {
                PolicyId    = policy.Id,
                VersionConfirmed = policy.Versions.LastActive().VersionNumber
            });
        }
    }
}