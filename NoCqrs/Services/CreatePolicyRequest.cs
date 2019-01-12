using System;

namespace NoCqrs.Services
{
    public class CreatePolicyRequest
    {
        public string OfferNumber { get; set; }
        public DateTime PurchaseDate { get; set; } 
        public DateTime PolicyStartDate { get; set; }
    }

    public class CreatePolicyResult
    {
        public string PolicyNumber { get; set; }
    }
}