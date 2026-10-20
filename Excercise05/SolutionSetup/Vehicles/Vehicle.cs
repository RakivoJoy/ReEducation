using System;

namespace SolutionSetup.Vehicles
{
    public abstract class Vehicle
    {
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int NumberOfWheels { get; set; }
        public abstract VehicleType Type { get; }

        protected Vehicle(string registrationNumber, string color, int numberOfWheels)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(registrationNumber);
            ArgumentException.ThrowIfNullOrWhiteSpace(color);
            ArgumentOutOfRangeException.ThrowIfNegative(numberOfWheels);

            RegistrationNumber = registrationNumber;
            Color = color;
            NumberOfWheels = numberOfWheels;
        }

        protected virtual string Details => string.Empty;

        public override string ToString()
        {
            string summary = $"{Type} {RegistrationNumber}, {Color}, {NumberOfWheels} wheels";
            return string.IsNullOrEmpty(Details) ? summary : $"{summary}, {Details}";
        }
    }
}
