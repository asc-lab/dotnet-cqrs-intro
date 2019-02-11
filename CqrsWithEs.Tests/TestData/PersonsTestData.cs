using System;
using CqrsWithEs.Domain;
using CqrsWithEs.Domain.Common;

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