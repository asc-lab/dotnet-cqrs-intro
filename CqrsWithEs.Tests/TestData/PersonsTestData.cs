using System;
using CqrsWithEs.Domain;

namespace CqrsWithEs.Tests
{
    public class PersonsTestData
    {
        public static Person Kowalski()
        {
            return new Person
            (
                "Jan",
                "Kowalski",
                "1111111116"
            );
        }
    }
}