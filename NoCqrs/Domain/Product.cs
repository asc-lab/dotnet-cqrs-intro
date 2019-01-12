using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NoCqrs.Domain
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        protected List<Cover> covers = new List<Cover>();
        public IReadOnlyCollection<Cover> Covers => new ReadOnlyCollection<Cover>(covers);

        public Product(Guid id, string code, string name, IList<Cover> covers)
        {
            Id = id;
            Code = code;
            Name = name;
            foreach (var cover in covers)
            {
                this.covers.Add(cover);    
            }
        }
    }
}