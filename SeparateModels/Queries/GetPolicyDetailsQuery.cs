using MediatR;
using SeparateModels.ReadModels;
using SeparateModels.Services;

namespace SeparateModels.Queries
{
    public class GetPolicyDetailsQuery : IRequest<PolicyVersionDto>
    {
        public string PolicyNumber { get; set; }
        public int VersionNumber { get; set; }
    }
}