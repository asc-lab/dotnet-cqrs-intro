using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.ReadModels;
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

        public async Task<PolicyDto> Handle(GetPolicyDetailsQuery query, CancellationToken cancellationToken)
        {
            var policy = await dataStore.Policies.WithNumber(query.PolicyNumber);
            
            return policy!=null ? PolicyDtoAssembler.AssemblePolicyDto(policy) : null;
        }
    }
}