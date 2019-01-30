using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.Services;

namespace SeparateModels.Queries
{
    public class GetPolicyDetailsHandler : IRequestHandler<GetPolicyDetailsQuery, PolicyDto>
    {
        private readonly IDataStore dataStore;

        public GetPolicyDetailsHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public Task<PolicyDto> Handle(GetPolicyDetailsQuery query, CancellationToken cancellationToken)
        {
            var policy = dataStore.Policies.WithNumber(query.PolicyNumber);
            
            return Task.FromResult(
                policy!=null ? PolicyDtoAssembler.AssemblePolicyDto(policy) : null
            );
        }
    }
}