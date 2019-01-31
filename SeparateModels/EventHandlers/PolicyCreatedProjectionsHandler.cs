using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using Npgsql;
using SeparateModels.Domain;
using SeparateModels.ReadModels;
using SeparateModels.Services;

namespace SeparateModels.EventHandlers
{
    public class PolicyCreatedProjectionsHandler : INotificationHandler<PolicyCreated>
    {
        public Task Handle(PolicyCreated @event, CancellationToken cancellationToken)
        {
            Console.WriteLine("Policy created " + @event.NewPolicy.Number);

            using (var cn =
                new NpgsqlConnection(
                    "User ID=lab_user;Password=lab_pass;Database=lab_cqrs_dotnet_demo;Host=localhost;Port=5432"))
            {
                var version = @event.NewPolicy.Versions.WithNumber(1);
                
                var policyInfo = new PolicyInfoDto
                {
                    PolicyId = @event.NewPolicy.Id,
                    PolicyNumber = @event.NewPolicy.Number,
                    CoverFrom = version.CoverPeriod.ValidFrom,
                    CoverTo = version.CoverPeriod.ValidTo,
                    PolicyHolder = $"{version.PolicyHolder.LastName} {version.PolicyHolder.FirstName}",
                    Vehicle = $"{version.Car.PlateNumber} {version.Car.Make}",
                    TotalPremiumAmount = version.TotalPremium.Amount
                };
                
                cn.Open();
                cn.Execute(
                    "INSERT INTO public.policy_info_view (policy_id,policy_number,cover_from,cover_to,vehicle,policy_holder,total_premium) " +
                    "VALUES (@PolicyId,@PolicyNumber,@CoverFrom,@CoverTo,@Vehicle,@PolicyHolder,@TotalPremiumAmount)",
                    policyInfo);
            }
            
            return Task.CompletedTask;
        }
    }
}