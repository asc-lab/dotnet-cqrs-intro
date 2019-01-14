using System;
using NoCqrs.Domain;

namespace NoCqrs.Init
{
    public class Persons
    {
        public static Person Kowalski()
        {
            return new Person
            (
                Guid.NewGuid(), 
                "Jan",
                "Kowalski",
                "1111111116"
            );
        }
    }
}