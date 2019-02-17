using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CqrsWithEs.Domain.Offer;
using CqrsWithEs.Domain.Policy;
using CqrsWithEs.Domain.Product;
using MediatR;

namespace CqrsWithEs.Commands
{
    public class BuyAdditionalCoverHandler : IRequestHandler<BuyAdditionalCoverCommand, BuyAdditionalCoverResult>
    {
        private readonly IPolicyRepository policyRepository;

        public BuyAdditionalCoverHandler(IPolicyRepository policyRepository)
        {
            this.policyRepository = policyRepository;
        }

        public Task<BuyAdditionalCoverResult> Handle(BuyAdditionalCoverCommand request, CancellationToken cancellationToken)
        {
            var policy = policyRepository.GetById(request.PolicyId);
            
            policy.ExtendCoverage
            (
                request.EffectiveDateOfChange, 
                new CoverPrice(request.NewCoverCode, request.NewCoverPrice, request.NewCoverPriceUnit)
            );
            
            policyRepository.Save(policy, policy.Version);
            
            return Task.FromResult
            (
                new BuyAdditionalCoverResult
                {
                    PolicyNumber = policy.PolicyNumber,
                    VersionWithAdditionalCoversNumber = policy.Versions.Last().VersionNumber
                }
            );
        }
    }
}