using System;
using MediatR;

namespace SeparateModels.Commands
{
    public class CreatePolicyCommand : IRequest<CreatePolicyResult>
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