using System;
using CqrsWithEs.Domain.Policy;
using Dapper;
using Npgsql;

namespace CqrsWithEs.ReadModel
{
    public class PolicyInfoDao
    {
        private readonly string cnString;

        public PolicyInfoDao(string cnString)
        {
            this.cnString = cnString;
        }

        public void CreatePolicyInfo
            (
                Guid id,
                string policyNumber,
                DateTime coverFrom,
                DateTime coverTo,
                string policyHolderFirstName, string policyHolderLastName,
                string carPlateNumber, string carMake,
                decimal totalPremium
            )
        {
            using (var cn = new NpgsqlConnection(cnString))
            {
                var policyInfo = new PolicyInfo
                {
                    PolicyId = id,
                    PolicyNumber = policyNumber,
                    CoverFrom = coverFrom,
                    CoverTo = coverTo,
                    PolicyHolder = $"{policyHolderLastName} {policyHolderFirstName}",
                    Vehicle = $"{carPlateNumber} {carMake}",
                    TotalPremiumAmount = totalPremium
                };
                
                cn.Open();
                cn.Execute(
                    "INSERT INTO public.policy_info_view (policy_id,policy_number,cover_from,cover_to,vehicle,policy_holder,total_premium) " +
                    "VALUES (@PolicyId,@PolicyNumber,@CoverFrom,@CoverTo,@Vehicle,@PolicyHolder,@TotalPremiumAmount)",
                    policyInfo);
            }    
        }

        public void UpdatePolicyInfo
            (
                Guid id,
                string policyNumber,
                DateTime coverFrom,
                DateTime coverTo,
                string policyHolderFirstName, string policyHolderLastName,
                string carPlateNumber, string carMake,
                decimal totalPremium
            )
        {
            using (var cn = new NpgsqlConnection(cnString))
            {
                var policyInfo = new PolicyInfo
                {
                    PolicyId = id,
                    CoverFrom = coverFrom,
                    CoverTo = coverTo,
                    PolicyHolder = $"{policyHolderLastName} {policyHolderFirstName}",
                    Vehicle = $"{carPlateNumber} {carMake}",
                    TotalPremiumAmount = totalPremium
                };

                
                cn.Open();
                cn.Execute(
                    "UPDATE public.policy_info_view " +
                    "SET " +
                    "cover_from = @CoverFrom, " +
                    "cover_to = @CoverTo, " +
                    "vehicle = @Vehicle, " +
                    "policy_holder = @PolicyHolder, " +
                    "total_premium = @TotalPremiumAmount " +
                    "WHERE policy_id = @PolicyId ",
                    policyInfo);
          
            }
        }
    }
}