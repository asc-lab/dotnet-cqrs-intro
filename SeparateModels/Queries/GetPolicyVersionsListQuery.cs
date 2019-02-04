using MediatR;
using SeparateModels.ReadModels;

namespace SeparateModels.Queries
{
    public class GetPolicyVersionsListQuery : IRequest<GetPolicyVersionsListResult>
    {
        public string PolicyNumber { get; set; }
    }

    public class GetPolicyVersionsListResult
    {
        public PolicyVersionsListDto Versions { get; set; }
    }
}