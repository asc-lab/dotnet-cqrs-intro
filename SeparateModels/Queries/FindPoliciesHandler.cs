using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Npgsql;
using SeparateModels.Domain;
using SeparateModels.ReadModels;
using SeparateModels.Services;

namespace SeparateModels.Queries
{
    public class FindPoliciesHandler : IRequestHandler<FindPoliciesQuery, List<PolicyInfoDto>>
    {
        public FindPoliciesHandler()
        {
        }

        public Task<List<PolicyInfoDto>> Handle(FindPoliciesQuery query, CancellationToken cancellationToken)
        {
            var policyFilter = new PolicyFilter
            (
                query.PolicyNumber,
                query.PolicyHolderFirstName,
                query.PolicyHolderLastName,
                query.PolicyStartFrom,
                query.PolicyStartTo,
                query.CarPlateNumber
            );

            using (var cn =
                new NpgsqlConnection(
                    "User ID=lab_user;Password=lab_pass;Database=lab_cqrs_dotnet_demo;Host=localhost;Port=5432"))
            {
                return Task.FromResult(cn.Query<PolicyInfoDto>("select " +
                                                               "policy_id as PolicyId," +
                                                               "policy_number as PolicyNumber," +
                                                               "cover_from as CoverFrom," +
                                                               "cover_to as CoverTo," +
                                                               "vehicle as Vehicle," +
                                                               "policy_holder as PolicyHolder," +
                                                               "total_premium as TotalPremiumAmount " +
                                                               "from public.policy_info_view").ToList());
            }

            
        }
    }
}