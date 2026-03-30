using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA2___Thiago_Braz
{
   public class ElectricBike : Bike
    {
        public int ChargeTime { get; set; }
        public int CurrentCharge { get; set; }

        public ElectricBike(string make, string model, DateTime dateOfManufacture, Engine engine, int chargeTime, int currentCharge)
        {
            Make = make;
            Model = model;
            DateOfManufacture = dateOfManufacture;
            Engine = engine;
            ChargeTime = chargeTime;
            CurrentCharge = currentCharge;
            Image = "electricbike.png";
        }

        public override string DisplayInfo()
        {
            return $"Make: {Make}\nModel: {Model}\nYear: {DateOfManufacture.Year}\n" +
                   $"Fuel: {Engine.Fuel}\nHorsepower: {Engine.HorsePower}\n" +
                   $"Charge Time: {ChargeTime} hours\nCurrent Charge: {CurrentCharge}%";
        }
    }
}
