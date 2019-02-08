namespace CqrsWithEs.Domain
{
    public class Car
    {
        public string Make { get; private set; }
        public string PlateNumber { get; private set; }
        public int ProductionYear { get; private set; }

        public Car(string make, string plateNumber, int productionYear)
        {
            Make = make;
            PlateNumber = plateNumber;
            ProductionYear = productionYear;
        }

        public Car Copy()
        {
            return new Car(Make, PlateNumber, ProductionYear);
        }
    }
}