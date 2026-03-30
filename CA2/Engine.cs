using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA2___Thiago_Braz
{
    public class Engine
    {
            public FuelType Fuel { get; set; }
            public int HorsePower { get; set; }

            public Engine(FuelType fuel, int horsepower)
            {
                Fuel = fuel;
                HorsePower = horsepower;
            }
    }
}
