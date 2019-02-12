using System;

namespace CqrsWithEs.Domain.Policy
{
    public interface IPolicyRepository
    {
        Policy GetById(Guid Id);
        void Save(Policy policy, int expectedVersion);
    }
}