using System.Linq;
using NoCqrs.Domain;

namespace NoCqrs.Services
{
    public static class PolicyDtoAssembler
    {
        public static PolicyDto AssemblePolicyDto(Policy policy)
        {
            return new PolicyDto
            {
                PolicyId = policy.Id,
                PolicyNumber = policy.Number,
                CurrentVersion = PolicyVersionDtoAssembler.AssemblePolicyVersionDto(policy.CurrentVersion),
                Versions = policy.Versions.Select(PolicyVersionDtoAssembler.AssemblePolicyVersionDto).ToList()
            };
        }
    }

    public static class PolicyVersionDtoAssembler
    {
        public static PolicyVersionDto AssemblePolicyVersionDto(PolicyVersion version)
        {
            return new PolicyVersionDto
            {
                VersionNumber = version.VersionNumber,
                VersionStatus = version.VersionStatus.ToString(),
                PolicyStatus = version.PolicyStatus.ToString(),
                PolicyHolder = $"{version.PolicyHolder.LastName} {version.PolicyHolder.FirstName}",
                Insured = null,
                Car = $"{version.Car.PlateNumber} {version.Car.Make} {version.Car.ProductionYear}",
                CoverFrom = version.CoverPeriod.ValidFrom,
                CoverTo = version.CoverPeriod.ValidTo,
                VersionFrom = version.VersionValidityPeriod.ValidFrom,
                VersionTo = version.VersionValidityPeriod.ValidTo,
                TotalPremium = version.TotalPremium.Amount,
                Covers = version.Covers.Select(CoverDtoAssembler.AssembleCoverDto).ToList()
            };
        }
    }

    public static class CoverDtoAssembler
    {
        public static CoverDto AssembleCoverDto(PolicyCover policyCover)
        {
            return new CoverDto
            {
                Code = policyCover.Cover.Code,
                CoverFrom = policyCover.CoverPeriod.ValidFrom,
                CoverTo = policyCover.CoverPeriod.ValidTo,
                PremiumAmount = policyCover.Amount.Amount
            };
        }
    }
}