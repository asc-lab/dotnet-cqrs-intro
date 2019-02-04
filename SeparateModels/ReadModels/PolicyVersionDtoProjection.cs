using System;
using Dapper;
using Marten.Linq.MatchesSql;
using Npgsql;
using SeparateModels.Domain;

namespace SeparateModels.ReadModels
{
    public class PolicyVersionDtoProjection
    {
        private readonly string cnString;

        public PolicyVersionDtoProjection(string cnString)
        {
            this.cnString = cnString;
        }

        public void CreatePolicyVersionDtoProjection(Policy policy, PolicyVersion policyVersion)
        {
            using (var cn = new NpgsqlConnection(cnString))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    cn.Execute
                    (
                        "insert into policy_version_view " +
                        "(policy_version_id, policy_id,policy_number,product_code,version_number," +
                        "version_status,policy_status,policy_holder,insured,car," +
                        "cover_from,cover_to,version_from,version_to,total_premium) " +
                        "values (@PolicyVersionId, @PolicyId, @PolicyNumber,@ProductCode,@VersionNumber," +
                        "@VersionStatus,@PolicyStatus,@PolicyHolder,@Insured,@Car," +
                        "@CoverFrom,@CoverTo,@VersionFrom,@VersionTo,@TotalPremium)", 
                        new
                        {
                            PolicyVersionId = policyVersion.Id,
                            PolicyId = policy.Id,
                            PolicyNumber = policy.Number,
                            ProductCode = policy.ProductCode,
                            VersionNumber = policyVersion.VersionNumber,
                            VersionStatus = policyVersion.VersionStatus.ToString(),
                            PolicyStatus = policyVersion.PolicyStatus.ToString(),
                            PolicyHolder = $"{policyVersion.PolicyHolder.LastName} {policyVersion.PolicyHolder.FirstName} {policyVersion.PolicyHolder.TaxId}",
                            Insured = "",
                            Car = $"{policyVersion.Car.PlateNumber} {policyVersion.Car.Make} {policyVersion.Car.ProductionYear}",
                            CoverFrom = policyVersion.CoverPeriod.ValidFrom,
                            CoverTo = policyVersion.CoverPeriod.ValidTo,
                            VersionFrom = policyVersion.VersionValidityPeriod.ValidFrom,
                            VersionTo = policyVersion.VersionValidityPeriod.ValidTo,
                            TotalPremium = policyVersion.TotalPremium.Amount
                        }
                    );

                    foreach (var cover in policyVersion.Covers)
                    {
                        cn.Execute
                        (
                            "insert into policy_version_cover " +
                            "(policy_version_cover_id,policy_version_id,code,cover_from,cover_to,premium_amount)" +
                            "values " +
                            "(@PolicyVersionCoverId,@PolicyVersionId,@Code,@CoverFrom,@CoverTo,@PremiumAmount)",
                            new
                            {
                                PolicyVersionCoverId = Guid.NewGuid(),
                                PolicyVersionId = policyVersion.Id,
                                Code = cover.CoverCode,
                                CoverFrom = cover.CoverPeriod.ValidFrom,
                                CoverTo = cover.CoverPeriod.ValidTo,
                                PremiumAmount = cover.Amount.Amount
                            }
                        );
                    }
                    
                    tx.Commit();
                }
            }
        }

        public void UpdatePolicyVersionDtoProjection(PolicyVersion policyVersion)
        {
            using (var cn = new NpgsqlConnection(cnString))
            {
                cn.Execute
                (
                    "update policy_version_view " +
                    "set version_status = @PolicyVersionStatus " +
                    "where policy_version_id = @PolicyVersionId",
                    new
                    {
                        PolicyVersionId = policyVersion.Id,
                        PolicyVersionStatus = policyVersion.VersionStatus.ToString()
                    }
                );
            }
        }
    }
}