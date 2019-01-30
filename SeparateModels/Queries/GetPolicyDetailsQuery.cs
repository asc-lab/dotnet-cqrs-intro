using MediatR;
using SeparateModels.Services;

namespace SeparateModels.Queries
{
    public class GetPolicyDetailsQuery : IRequest<PolicyDto>
    {
        public string PolicyNumber { get; set; }
    }
}