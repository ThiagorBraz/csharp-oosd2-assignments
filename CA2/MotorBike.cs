using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA2___Thiago_Braz
{
    public class MotorBike : Bike
    {
        public int TankCapacity { get; set; }
        public int MaxSpeed { get; set; }

        public MotorBike(string make, string model, DateTime dateOfManufacture, Engine engine, int tankCapacity, int maxSpeed)
        {
            Make = make;
            Model = model;
            DateOfManufacture = dateOfManufacture;
            Engine = engine;
            TankCapacity = tankCapacity;
            MaxSpeed = maxSpeed;
            Image = "motorbike.png";
        }

        public override string DisplayInfo()
        {
            return $"Make: {Make}\nModel: {Model}\nYear: {DateOfManufacture.Year}\n" +
                   $"Fuel: {Engine.Fuel}\nHorsepower: {Engine.HorsePower}\n" +
                   $"Tank Capacity: {TankCapacity} litres\nMax Speed: {MaxSpeed} kph";
        }
    }
}
