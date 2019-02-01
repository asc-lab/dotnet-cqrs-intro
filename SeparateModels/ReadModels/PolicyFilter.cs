using System;

namespace SeparateModels.ReadModels
{
    public class PolicyFilter
    {
        public string PolicyNumber { get; private set; }
        public string PolicyHolder{ get; private set; }
        public DateTime? PolicyStartDateFrom { get; private set; }
        public DateTime? PolicyStartDateTo { get; private set; }
        public string CarPlateNumber { get; private set; }

        public PolicyFilter(string policyNumber, string policyHolder, DateTime? policyStartDateFrom, DateTime? policyStartDateTo, string carPlateNumber)
        {
            PolicyNumber = policyNumber;
            PolicyHolder = policyHolder;
            PolicyStartDateFrom = policyStartDateFrom;
            PolicyStartDateTo = policyStartDateTo;
            CarPlateNumber = carPlateNumber;
        }
    }
}