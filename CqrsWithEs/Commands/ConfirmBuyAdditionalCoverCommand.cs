using System;
using MediatR;

namespace CqrsWithEs.Commands
{
    public class ConfirmBuyAdditionalCoverCommand : IRequest<ConfirmBuyAdditionalCoverResult>
    {
        public Guid PolicyId { get; set; }
        public int VersionToConfirmNumber { get; set; }
    }

    public class ConfirmBuyAdditionalCoverResult
    {
        public Guid PolicyId { get; set; }
        public int VersionConfirmed { get; set; }
    }
}