using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CqrsWithEs.Domain.Policy.Events;
using CqrsWithEs.ReadModel;
using MediatR;

namespace CqrsWithEs.EventHandlers
{
    public class PolicyInfoSynchronizer : 
        INotificationHandler<InitialPolicyVersionCreated>,
        INotificationHandler<CoverageExtendedPolicyVersionConfirmed>,
        INotificationHandler<TerminalPolicyVersionConfirmed>
    {
        private readonly PolicyInfoDao policyInfoDao;

        public PolicyInfoSynchronizer(PolicyInfoDao policyInfoDao)
        {
            this.policyInfoDao = policyInfoDao;
        }

        public Task Handle(InitialPolicyVersionCreated @event, CancellationToken cancellationToken)
        {
            policyInfoDao.CreatePolicyInfo
            (
                Guid.NewGuid(), 
                @event.PolicyNumber,
                @event.CoverFrom,
                @event.CoverTo,
                @event.PolicyHolder.FirstName, @event.PolicyHolder.LastName,
                @event.Car.PlateNumber, @event.Car.Make,
                @event.Covers.Select(c=>c.Amount.Amount).Sum()
            );
            
            return Task.CompletedTask;
        }

        public Task Handle(CoverageExtendedPolicyVersionConfirmed @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Handle(TerminalPolicyVersionConfirmed notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}