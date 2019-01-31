using MediatR;

namespace SeparateModels.Domain
{
    public class PolicyCreated : INotification
    {
        public Policy NewPolicy { get; }

        public PolicyCreated(Policy newPolicy)
        {
            NewPolicy = newPolicy;
        }
    }
}