using System;
using MediatR;

namespace CqrsWithEs.Commands
{
    public class TerminatePolicyCommand : IRequest<TerminatePolicyResult>
    {
        public Guid PolicyId { get; set; }
        public DateTime TerminationDate { get; set; }
    }

    public class TerminatePolicyResult
    {
        public Guid PolicyId { get; set; }
        public int VersionWithTerminationNumber { get; set; }
    }
}