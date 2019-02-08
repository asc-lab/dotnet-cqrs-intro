using System;
using CqrsWithEs.Domain;

namespace CqrsWithEs.Tests
{
    public class CarsTestData
    {
        public static Car OldFordFocus()
        {
            return new Car
            (
                "Ford Focus",
                "WAW1010",
                2005
            );
        }
    }
}