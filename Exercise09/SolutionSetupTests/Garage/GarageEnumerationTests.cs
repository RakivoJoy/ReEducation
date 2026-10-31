using SolutionSetup;
using SolutionSetup.Vehicles;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SolutionSetupTests
{
    public class GarageEnumerationTests
    {
        [Fact]
        public void EmptyGarage_EnumeratesNothing()
        {
            // Arrange & Act & Assert
            Assert.Empty(new Garage<Car>(2));
        }

        [Fact]
        public void Garage_SupportsLinq()
        {
            // Arrange
            var garage = new Garage<IVehicle>(3);
            garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));
            garage.AddVehicle(new Bus("DEF456", "Yellow", 6, 50));

            // Act & Assert
            Assert.Single(garage, v => v.NumberOfWheels == 6);
            Assert.Equal(10, garage.Sum(v => v.NumberOfWheels));
        }

        [Fact]
        public void NonGenericEnumeration_Works()
        {
            // Arrange
            var garage = new Garage<Car>(1);
            garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));

            // Act
            IEnumerable nonGeneric = garage;
            int count = 0;
            foreach (object _ in nonGeneric)
            {
                count++;
            }

            // Assert
            Assert.Equal(1, count);
        }

        [Fact]
        public void Garage_OfConcreteType_ReturnsConcreteMembers()
        {
            // Arrange
            var garage = new Garage<Car>(1);
            var car = new Car("ABC123", "Black", 4, FuelType.Gasoline);
            garage.AddVehicle(car);

            // Act
            Car? found = garage.FindByRegistration("abc123");

            // Assert
            Assert.Same(car, found);
            Assert.Same(car, garage.ListAllVehicles()[0]);
        }

        [Fact]
        public void Garage_Enumeration_ReflectsCurrentState()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);
            garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));

            // Act
            garage.RemoveVehicleByRegistration("ABC123");

            // Assert
            Assert.Empty(garage);
        }
    }
}
