using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CqrsWithEs.Domain.Policy;
using MediatR;

namespace CqrsWithEs.Commands
{
    public class TerminatePolicyHandler : IRequestHandler<TerminatePolicyCommand, TerminatePolicyResult>
    {
        private readonly IPolicyRepository policyRepository;

        public TerminatePolicyHandler(IPolicyRepository policyRepository)
        {
            this.policyRepository = policyRepository;
        }
        

        public Task<TerminatePolicyResult> Handle(TerminatePolicyCommand request, CancellationToken cancellationToken)
        {
            var policy = policyRepository.GetById(request.PolicyId);
            
            policy.TerminatePolicy(request.TerminationDate);
            
            policyRepository.Save(policy, policy.Version);
            
            return Task.FromResult(new TerminatePolicyResult
            {
                PolicyId = policy.Id,
                VersionWithTerminationNumber = policy.Versions.Last().VersionNumber
            });
        }

    }
}