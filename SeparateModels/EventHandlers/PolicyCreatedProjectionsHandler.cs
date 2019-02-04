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
    public class PolicyCreatedProjectionsHandler : 
        INotificationHandler<PolicyCreated>
    {
        private readonly PolicyInfoDtoProjection policyInfoDtoProjection;
        private readonly PolicyVersionDtoProjection policyVersionDtoProjection;

        public PolicyCreatedProjectionsHandler(PolicyInfoDtoProjection policyInfoDtoProjection, PolicyVersionDtoProjection policyVersionDtoProjection)
        {
            this.policyInfoDtoProjection = policyInfoDtoProjection;
            this.policyVersionDtoProjection = policyVersionDtoProjection;
        }

        public Task Handle(PolicyCreated @event, CancellationToken cancellationToken)
        {
            policyInfoDtoProjection.CreatePolicyInfoDto(@event.NewPolicy);
            
            policyVersionDtoProjection.CreatePolicyVersionDtoProjection(@event.NewPolicy, @event.NewPolicy.Versions.WithNumber(1));
            
            return Task.CompletedTask;
        }
    }
}