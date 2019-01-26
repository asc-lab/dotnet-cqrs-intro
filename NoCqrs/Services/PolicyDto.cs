using System;
using System.Collections.Generic;

namespace NoCqrs.Services
{
    public class PolicyDto
    {
        public Guid PolicyId { get; set; }
        
        public string PolicyNumber { get; set; }
        
        public PolicyVersionDto CurrentVersion { get; set; }
        
        public List<PolicyVersionDto> Versions { get; set; }
    }

    public class PolicyVersionDto
    {
        public int VersionNumber { get; set; }
        public string VersionStatus { get; set; }
        public string PolicyStatus { get; set; }
        public string PolicyHolder { get; set; }
        public string Insured { get; set; }
        public string Car { get; set; }
        public DateTime CoverFrom { get; set; }
        public DateTime CoverTo { get; set; }
        public DateTime VersionFrom { get; set; }
        public DateTime VersionTo { get; set; }
        public decimal TotalPremium { get; set; }
        public List<CoverDto> Covers { get; set; }
    }

    public class CoverDto
    {
        public string Code { get; set; }
        public DateTime CoverFrom { get; set; }
        public DateTime CoverTo { get; set; }
        public decimal PremiumAmount { get; set; }
    }
}