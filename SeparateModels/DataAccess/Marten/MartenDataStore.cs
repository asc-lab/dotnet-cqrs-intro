using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Marten;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SeparateModels.Domain;

namespace SeparateModels.DataAccess.Marten
{
    public class MartenDataStore : IDataStore //, IDisposable
    {
        private readonly IDocumentSession session;

        public IProductRepository Products { get; }
        public IOfferRepository Offers { get; }
        public IPolicyRepository Policies { get; }
        
        public MartenDataStore(IDocumentStore documentStore)
        {
            session = documentStore.LightweightSession();
            Products = new ProductRepository(session);
            Offers = new OfferRepository(session);
            Policies = new PolicyRepository(session);
        }  
        
        public async Task CommitChanges()
        {
            await session.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //session.Dispose();
            }
            
        }
    }

    public class ProductRepository : IProductRepository
    {
        private readonly IDocumentSession session;

        public ProductRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public void Add(Product product)
        {
            session.Insert(product);
        }

        public Product WithCode(string code)
        {
            return session.Query<Product>().FirstOrDefault(p => p.Code == code);
        }

        //TODO: get rid of it
        public List<Product> All()
        {
            return session.Query<Product>().ToList();
        }
    }

    public class OfferRepository : IOfferRepository
    {
        private readonly IDocumentSession session;

        public OfferRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public Offer WithNumber(string number)
        {
            return session.Query<Offer>().FirstOrDefault(o => o.Number == number);
        }

        //TODO: get rid of it
        public List<Offer> All()
        {
            return session.Query<Offer>().ToList();
        }

        public void Add(Offer offer)
        {
            session.Insert(offer);
        }
    }
    
    public class PolicyRepository : IPolicyRepository
    {
        private readonly IDocumentSession session;

        public PolicyRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public Policy WithNumber(string number)
        {
            return session.Query<Policy>().FirstOrDefault(p => p.Number == number);
        }

        public void Add(Policy policy)
        {
            session.Insert(policy);
        }

        //TODO: get rid of it
        public IList<Policy> Find(PolicyFilter filter)
        {
            throw new System.NotImplementedException();
        }
    }
}