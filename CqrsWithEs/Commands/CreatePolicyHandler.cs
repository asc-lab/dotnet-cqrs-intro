using System.Threading;
using System.Threading.Tasks;
using CqrsWithEs.Domain;
using MediatR;

namespace CqrsWithEs.Commands
{
    public class CreatePolicyHandler : IRequestHandler<CreatePolicyCommand, CreatePolicyResult>
    {
        private IOfferRepository offerRepository;

        public CreatePolicyHandler(IOfferRepository offerRepository)
        {
            this.offerRepository = offerRepository;
        }

        public Task<CreatePolicyResult> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
        {
            var offer = offerRepository.WithNumber(request.OfferNumber);
            
            return Task.FromResult(new CreatePolicyResult
            {
                PolicyNumber = null
            });
        }
    }
}