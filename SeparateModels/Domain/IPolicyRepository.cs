using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeparateModels.Domain
{
    public interface IPolicyRepository
    {
        Task<Policy> WithNumber(string number);

        void Add(Policy policy);

        Task<IList<Policy>> Find(PolicyFilter filter);
    }

    public class PolicyFilter
    {
        public string PolicyNumber { get; private set; }
        public string PolicyHolderFirstName { get; private set; }
        public string PolicyHolderLastName { get; private set; }
        public DateTime? PolicyStartDateFrom { get; private set; }
        public DateTime? PolicyStartDateTo { get; private set; }
        public string CarPlateNumber { get; private set; }

        public PolicyFilter(string policyNumber, string policyHolderFirstName, string policyHolderLastName, DateTime? policyStartDateFrom, DateTime? policyStartDateTo, string carPlateNumber)
        {
            PolicyNumber = policyNumber;
            PolicyHolderFirstName = policyHolderFirstName;
            PolicyHolderLastName = policyHolderLastName;
            PolicyStartDateFrom = policyStartDateFrom;
            PolicyStartDateTo = policyStartDateTo;
            CarPlateNumber = carPlateNumber;
        }
    }
}