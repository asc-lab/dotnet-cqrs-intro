using System;
using System.Collections.Generic;

namespace SeparateModels.Domain
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        private List<Cover> covers = new List<Cover>();
        public IEnumerable<Cover> Covers => covers.AsReadOnly();

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

        //required by EF
        protected Product()
        {
        }
    }
}