using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA2___Thiago_Braz
{
    public class TraditionalBike : Bike
    {
        public bool HasCarrier { get; set; }
        public bool HasBell { get; set; }

        public TraditionalBike(string make, string model, DateTime dateOfManufacture, bool hasBell, bool hasCarrier)
        {
            Make = make;
            Model = model;
            DateOfManufacture = dateOfManufacture;
            Engine = new Engine(FuelType.Human, 0);
            HasBell = hasBell;
            HasCarrier = hasCarrier;
            Image = "bike.png";
        }

        public override string DisplayInfo()
        {
            return $"Make: {Make}\nModel: {Model}\nYear: {DateOfManufacture.Year}\n" +
                   $"Fuel: {Engine.Fuel}\nHorsepower: {Engine.HorsePower}\n" +
                   $"Has a bell: {(HasBell ? "Yes" : "No")}\nHas a carrier: {(HasCarrier ? "Yes" : "No")}";
        }
    }
}
