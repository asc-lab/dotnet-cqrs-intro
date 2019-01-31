using SeparateModels.Domain;

namespace SeparateModels.ReadModels
{
    public class PolicyInfoDtoAssembler
    {
        public static PolicyInfoDto AssemblePolicyInfoDto(Policy policy, PolicyVersion policyVersion)
        {
            return new PolicyInfoDto
            {
                PolicyId = policy.Id,
                PolicyNumber = policy.Number,
                CoverFrom = policyVersion.CoverPeriod.ValidFrom,
                CoverTo = policyVersion.CoverPeriod.ValidTo,
                Vehicle = AssembleVehicleDescription(policyVersion.Car),
                PolicyHolder = AssemblePolicyHolderDescription(policyVersion.PolicyHolder),
                TotalPremiumAmount = policyVersion.TotalPremium.Amount
            };
        }

        private static string AssemblePolicyHolderDescription(Person policyHolder)
        {
            return $"{policyHolder.LastName} {policyHolder.FirstName}";
        }

        private static string AssembleVehicleDescription(Car car)
        {
            return $"{car.Make} {car.ProductionYear} {car.PlateNumber}";
        }
    }
}