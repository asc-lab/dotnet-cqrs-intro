using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeparateModels.Domain
{
    public interface IPolicyRepository
    {
        Task<Policy> WithNumber(string number);

        void Add(Policy policy);
    }
}