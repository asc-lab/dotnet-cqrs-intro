using System;

namespace NoCqrs.Domain
{
    public class Car
    {
        public Guid Id { get; private set; }
        public string Make { get; private set; }
        public string PlateNumber { get; private set; }
        public int ProductionYear { get; private set; }

        public Car(Guid id, string make, string plateNumber, int productionYear)
        {
            Id = id;
            Make = make;
            PlateNumber = plateNumber;
            ProductionYear = productionYear;
        }
    }
}