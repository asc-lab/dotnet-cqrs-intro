using System;
using MediatR;

namespace CqrsWithEs.Commands
{
    public class ConfirmTerminationCommand : IRequest<ConfirmTerminationResult>
    {
        public Guid PolicyId { get; set; }
        public int VersionToConfirmNumber { get; set; }
    }

    public class ConfirmTerminationResult
    {
        public Guid PolicyId { get; set; }
        public int VersionConfirmed { get; set; }
    }
}