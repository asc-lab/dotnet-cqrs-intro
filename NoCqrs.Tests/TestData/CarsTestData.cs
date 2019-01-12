using System;
using NoCqrs.Domain;

namespace NoCqrs.Tests
{
    public class CarsTestData
    {
        public static Car OldFordFocus()
        {
            return new Car
            (
                Guid.NewGuid(),
                "Ford Focus",
                "WAW1010",
                2005
            );
        }
    }
}