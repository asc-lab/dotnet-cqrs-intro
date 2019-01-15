using System;
using NoCqrs.Domain;

namespace NoCqrs.Tests
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