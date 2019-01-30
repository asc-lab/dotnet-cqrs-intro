using SeparateModels.Domain;

namespace SeparateModels.Init
{
    public class Cars
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