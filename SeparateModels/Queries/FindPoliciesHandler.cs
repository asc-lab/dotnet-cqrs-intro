using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.ReadModels;
using SeparateModels.Services;

namespace SeparateModels.Queries
{
    public class FindPoliciesHandler : IRequestHandler<FindPoliciesQuery, List<PolicyInfoDto>>
    {
        private readonly IDataStore dataStore;
    
        public FindPoliciesHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task<List<PolicyInfoDto>> Handle(FindPoliciesQuery query, CancellationToken cancellationToken)
        {
            var policyFilter = new PolicyFilter
            (
                query.PolicyNumber,
                query.PolicyHolderFirstName,
                query.PolicyHolderLastName,
                query.PolicyStartFrom,
                query.PolicyStartTo,
                query.CarPlateNumber
            );

            var results = await dataStore.Policies.Find(policyFilter);

            return results
                .Select(p => PolicyInfoDtoAssembler.AssemblePolicyInfoDto(p, p.CurrentVersion))
                .ToList();
        }
    }
}