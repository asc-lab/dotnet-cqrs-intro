using System;

namespace NoCqrs.Services
{
    public class SearchPolicyRequest
    {
        public string PolicyNumber { get; set; }
        public DateTime? PolicyStartFrom { get; set; }
        public DateTime? PolicyStartTo { get; set; }
        public string CarPlateNumber { get; set; }
        public string PolicyHolderFirstName { get; set; }
        public string PolicyHolderLastName { get; set; }
    }
}