using System.Collections.Generic;
using System.Linq;
using CqrsWithEs.Domain.Common;
using NodaMoney;

namespace CqrsWithEs.Domain.Policy
{
    public interface IPolicyState
    {
        PolicyStatus PolicyStatus { get; }
        ValidityPeriod CoverPeriod { get; }
        ValidityPeriod VersionPeriod { get; }
        IReadOnlyCollection<PolicyCover> Covers { get; }
        Money TotalPremium { get; }
    }
}