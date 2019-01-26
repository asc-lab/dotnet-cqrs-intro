using System;
using System.Collections;
using System.Collections.Generic;

namespace NoCqrs.Domain
{
    public interface IPolicyRepository
    {
        Policy WithNumber(string number);

        void Add(Policy policy);

        IList<Policy> Find(PolicyFilter filter);
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