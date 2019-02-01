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
        private readonly PolicyInfoDtoProjection policyInfoDtoProjection;

        public PolicyCreatedProjectionsHandler(PolicyInfoDtoProjection policyInfoDtoProjection)
        {
            this.policyInfoDtoProjection = policyInfoDtoProjection;
        }

        public Task Handle(PolicyCreated @event, CancellationToken cancellationToken)
        {
            policyInfoDtoProjection.CreatePolicyInfoDto(@event.NewPolicy);
            
            return Task.CompletedTask;
        }
    }
}