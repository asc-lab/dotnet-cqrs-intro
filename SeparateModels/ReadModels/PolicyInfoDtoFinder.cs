using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Npgsql;
using static System.String;

namespace SeparateModels.ReadModels
{
    public class PolicyInfoDtoFinder
    {
        private readonly string cnString;

        private readonly string baseQuery = "select " +
                                            "policy_id as PolicyId," +
                                            "policy_number as PolicyNumber," +
                                            "cover_from as CoverFrom," +
                                            "cover_to as CoverTo," +
                                            "vehicle as Vehicle," +
                                            "policy_holder as PolicyHolder," +
                                            "total_premium as TotalPremiumAmount " +
                                            "from public.policy_info_view " +
                                            "where 1 = 1 ";

        public PolicyInfoDtoFinder(string cnString)
        {
            this.cnString = cnString;
        }

        public IList<PolicyInfoDto> FindByFilter(PolicyFilter filter)
        {
            using (var cn = new NpgsqlConnection(cnString))
            {
                var sql = BuildSql(filter);
                return cn
                    .Query<PolicyInfoDto>(sql, filter)
                    .ToList();
            }
        }

        private string BuildSql(PolicyFilter filter)
        {
            var sql = new StringBuilder();

            sql.Append(baseQuery);

            if (!IsNullOrWhiteSpace(filter.PolicyNumber))
            {
                sql.Append("and policy_number = @PolicyNumber");
            }

            if (filter.PolicyStartDateFrom.HasValue)
            {
                sql.Append("and cover_from >= @PolicyStartDateFrom");
            }
            
            if (filter.PolicyStartDateTo.HasValue)
            {
                sql.Append("and cover_to <= @PolicyStartDateTo");
            }

            if (!IsNullOrWhiteSpace(filter.CarPlateNumber))
            {
                sql.Append("and vehicle LIKE @CarPlateNumber || ' %'");
            }

            if (!IsNullOrWhiteSpace(filter.PolicyHolder))
            {
                sql.Append("and policy_holder = @PolicyHolder");
            }

            return sql.ToString();
        }
    }
}