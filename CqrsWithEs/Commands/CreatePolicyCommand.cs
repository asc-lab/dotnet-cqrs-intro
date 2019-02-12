using System;
using MediatR;

namespace CqrsWithEs.Commands
{
    public class CreatePolicyCommand : IRequest<CreatePolicyResult>
    {
        public string OfferNumber { get; set; }
        public DateTime PurchaseDate { get; set; } 
        public DateTime PolicyStartDate { get; set; }
    }

    public class CreatePolicyResult
    {
        public Guid PolicyId { get; set; }
        public string PolicyNumber { get; set; }
    }
}