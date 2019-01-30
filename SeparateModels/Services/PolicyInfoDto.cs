using System;

namespace SeparateModels.Services
{
    public class PolicyInfoDto
    {
        public Guid PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime CoverFrom { get; set; }
        public DateTime CoverTo { get; set; }
        public string Vehicle { get; set; }
        public string PolicyHolder { get; set; }
        public decimal TotalPremiumAmount { get; set; }
    }
}