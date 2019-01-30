using System;
using MediatR;

namespace SeparateModels.Commands
{
    public class TerminatePolicyCommand : IRequest<TerminatePolicyResult>
    {
        public string PolicyNumber { get; set; }
        public DateTime TerminationDate { get; set; }
    }

    public class TerminatePolicyResult
    {
        public string PolicyNumber { get; set; }
        public int VersionWithTerminationNumber { get; set; }
    }
}