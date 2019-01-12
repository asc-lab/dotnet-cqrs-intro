using System;

namespace NoCqrs.Domain
{
    public class Person
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string TaxId { get; private set; }

        public Person(Guid id, string firstName, string lastName, string taxId)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            TaxId = taxId;
        }
    }
}