using System;

namespace CqrsWithEs.Domain.Product
{
    public class Cover
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }

        public Cover(Guid id, string code, string name)
        {
            Id = id;
            Code = code;
            Name = name;
        }

        //required by EF
        protected Cover()
        {
        }
    }
    
    
}