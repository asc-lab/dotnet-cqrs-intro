using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.ReadModels;
using SeparateModels.Services;

namespace SeparateModels.Queries
{
    public class GetPolicyDetailsHandler : IRequestHandler<GetPolicyDetailsQuery, PolicyVersionDto>
    {
        private readonly IDataStore dataStore;

        public GetPolicyDetailsHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public Task<PolicyVersionDto> Handle(GetPolicyDetailsQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(new PolicyVersionDto());
        }
    }
}