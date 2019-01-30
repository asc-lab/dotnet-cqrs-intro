using System;
using MediatR;

namespace SeparateModels.Commands
{
    public class BuyAdditionalCoverCommand : IRequest<BuyAdditionalCoverResult>
    {
        public string PolicyNumber { get; set; }
        public DateTime EffectiveDateOfChange { get; set; }
        public string NewCoverCode { get; set; }
        public decimal NewCoverPrice { get; set; }
        public TimeSpan NewCoverPriceUnit { get; set; }
    }
    
    public class BuyAdditionalCoverResult
    {
        public string PolicyNumber { get; set; }
        public int VersionWithAdditionalCoversNumber { get; set; }
    }
}