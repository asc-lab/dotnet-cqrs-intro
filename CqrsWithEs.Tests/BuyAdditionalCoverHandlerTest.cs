using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;
using CqrsWithEs.Commands;
using CqrsWithEs.DataAccess;
using CqrsWithEs.Domain.Base;
using CqrsWithEs.Domain.Policy.Events;
using NodaMoney;
using Xunit;

namespace CqrsWithEs.Tests
{
    public class BuyAdditionalCoverHandlerTest
    {
        [Fact]
        public async void CanBuyAdditionalCover()
        {
            var policyId = new Guid("6522675f-710b-4614-985a-1a1d938a0a2c");
            var handler = new BuyAdditionalCoverHandler
            (
                new InMemoryPolicyRepository
                (
                    StoreWithEvents(policyId, new List<Event>
                    {
                        new InitialPolicyVersionCreated
                        (
                            "POL001",
                            "P1",
                            new DateTime(2019,1,1),
                            new DateTime(2020,1,1),
                            new DateTime(2019,1,1),
                            new PersonData("A","A","A"),
                            new CarData("A","A",2010),
                            new List<PolicyCoverData>
                            {
                                new PolicyCoverData("OC", new DateTime(2019,1,1),new DateTime(2020,1,1), Money.Euro(500), Money.Euro(500),TimeSpan.FromDays(365))
                            }
                        )
                    })
                )
            );

            var cmdResult = await handler.Handle(
                new BuyAdditionalCoverCommand
                {
                    PolicyId = policyId,
                    NewCoverCode = "AC",
                    NewCoverPrice = 200M,
                    NewCoverPriceUnit = TimeSpan.FromDays(365),
                    EffectiveDateOfChange = new DateTime(2019,6,1)
                }, 
                CancellationToken.None);
            
            Assert.Equal(2, cmdResult.VersionWithAdditionalCoversNumber);
        }


        private IEventStore StoreWithEvents(Guid id, IEnumerable<Event> events)
        {
            var store = new EventStore(null);
            store.SaveEvents(id, events, -1);
            return store;
        }
    }
}