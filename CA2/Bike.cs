using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA2___Thiago_Braz
{
    public abstract class Bike
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime DateOfManufacture { get; set; }
        public Engine Engine { get; set; }
        public string Image { get; set; }

        public abstract string DisplayInfo();

        public override string ToString()
        {
            return $"{Make} {Model} ({this.GetType().Name.ToUpper()})";
        }
    }
}
