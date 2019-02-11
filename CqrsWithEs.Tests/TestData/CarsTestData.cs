using System;
using CqrsWithEs.Domain;
using CqrsWithEs.Domain.Common;

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