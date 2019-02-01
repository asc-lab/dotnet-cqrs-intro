using System;
using System.Collections.Generic;

namespace SeparateModels.ReadModels
{
    public class PolicyVersionDto
    {
        public Guid PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string ProductCode { get; set; }
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
        public List<string> Changes { get; set; }
    }

    public class CoverDto
    {
        public string Code { get; set; }
        public DateTime CoverFrom { get; set; }
        public DateTime CoverTo { get; set; }
        public decimal PremiumAmount { get; set; }
    }
}