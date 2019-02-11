using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CqrsWithEs.Domain.Product
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        [JsonProperty]
        private List<Cover> covers = new List<Cover>();
        [JsonIgnore]
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

        //required by JSON
        public Product()
        {
        }

        public Cover CoverWithCode(string coverCode)
        {
            return covers.FirstOrDefault(c => c.Code == coverCode);
        }
    }
}