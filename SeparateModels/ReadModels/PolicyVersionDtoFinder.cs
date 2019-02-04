using System.Linq;
using Dapper;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Npgsql;

namespace SeparateModels.ReadModels
{
    public class PolicyVersionDtoFinder
    {
        private readonly string cnString;

        private const string getVersionQuery = "select policy_version_id as Id, " +
                                               "    policy_id as PolicyId , " +
                                               "    policy_number as PolicyNumber, " +
                                               "    product_code as ProductCode, " +
                                               "    version_number as VersionNumber, " +
                                               "    version_status as VersionStatus, " +
                                               "    policy_status as PolicyStatus, " +
                                               "    policy_holder as PolicyHolder, " +
                                               "    insured as Insured, " +
                                               "    car as Car," +
                                               "    cover_from as CoverFrom, " +
                                               "    cover_to as CoverTo, " +
                                               "    version_from as VersionFrom, " +
                                               "    version_to as VersionTo, " +
                                               "    total_premium as TotalPremium " +
                                               "from policy_version_view " +
                                               "where policy_number = @PolicyNumber and version_number = @VersionNumber";
        
        private const string getCoversInVersion = "select policy_version_cover_id as Id, " +
                                                  "    code as Code, " +
                                                  "    cover_from as CoverFrom, " +
                                                  "    cover_to as CoverTo, " +
                                                  "    premium_amount as PremiumAmount " +
                                                  "from policy_version_cover " +
                                                  "where policy_version_id = @PolicyVersionId";

        private const string getVersionsListQuery = "select version_number Number, " +
                                                    "    version_from as VersionFrom, " +
                                                    "    version_to as VersionTo, " +
                                                    "    version_status as VersionStatus " +
                                                    "from policy_version_view " +
                                                    "where policy_number = @PolicyNumber";
        
        public PolicyVersionDtoFinder(string cnString)
        {
            this.cnString = cnString;
        }

        public PolicyVersionDto FindByPolicyNumberAndVersionNumber(string policyNumber, int versionNumber)
        {
            using (var cn = new NpgsqlConnection(cnString))
            {
                var policyVersion = cn
                    .Query<PolicyVersionDto>(getVersionQuery, new {PolicyNumber = policyNumber, VersionNumber = versionNumber})
                    .FirstOrDefault();

                var covers = cn
                    .Query<CoverDto>(getCoversInVersion, new {PolicyVersionId = policyVersion.Id})
                    .ToList();

                policyVersion.Covers = covers;
                
                return policyVersion;
            }
        }

        public PolicyVersionsListDto FindVersionsListByPolicyNumber(string policyNumber)
        {
            using (var cn = new NpgsqlConnection(cnString))
            {
                var versions = cn
                    .Query<PolicyVersionInfoDto>
                    (
                        getVersionsListQuery,
                        new {PolicyNumber = policyNumber}
                    )
                    .ToList();
                
                return new PolicyVersionsListDto
                {
                    PolicyNumber = policyNumber,
                    VersionsInfo = versions
                };
            }
        }
    }
}