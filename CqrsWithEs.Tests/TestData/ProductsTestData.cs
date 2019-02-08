using System;
using System.Collections.Generic;
using CqrsWithEs.Domain;

namespace CqrsWithEs.Tests
{
    public class ProductsTestData
    {
        public static Product StandardCarInsurance()
        {
            return new Product
            (
                Guid.NewGuid(),
                "STD_CAR_INSURANCE",
                "Standard Car Insurance",
                new List<Cover>
                {
                    new Cover(Guid.NewGuid(), "OC", "Third party liability"),
                    new Cover(Guid.NewGuid(), "AC", "Auto casco"),
                    new Cover(Guid.NewGuid(), "ASSISTANCE", "Assistance")
                }
            );
        }
    }
}