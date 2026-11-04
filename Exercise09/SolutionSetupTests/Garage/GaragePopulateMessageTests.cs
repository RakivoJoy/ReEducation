using SolutionSetup;
using SolutionSetup.Vehicles;
using System.Collections.Generic;
using System.Linq;

namespace SolutionSetupTests
{
    public class GaragePopulateMessageTests
    {
        [Fact]
        public void Populate_ReportsReason_WhenGarageIsFull()
        {
            // Arrange
            var garage = new Garage<IVehicle>(1);
            var vehicles = new List<IVehicle>
            {
                new Car("ABC123", "Black", 4, FuelType.Gasoline),
                new Car("ABC124", "Red", 4, FuelType.Diesel)
            };

            // Act
            var result = garage.Populate(vehicles);

            // Assert
            Assert.Single(result.Failures);
            Assert.Equal("ABC124", result.Failures[0].RegistrationNumber);
            Assert.Equal(ParkFailureReason.Full, result.Failures[0].Reason);
        }

        [Fact]
        public void Populate_ReportsReason_WhenVehicleAlreadyParked()
        {
            // Arrange
            var garage = new Garage<IVehicle>(3);
            garage.AddVehicle(new Car("ABC123", "Black", 4, FuelType.Gasoline));
            var vehicles = new List<IVehicle> { new Car("ABC123", "Black", 4, FuelType.Gasoline) };

            // Act
            var result = garage.Populate(vehicles);

            // Assert
            Assert.Single(result.Failures);
            Assert.Equal(ParkFailureReason.Duplicate, result.Failures[0].Reason);
        }

        [Fact]
        public void Populate_ListsEveryFailedVehicle()
        {
            // Arrange
            var garage = new Garage<IVehicle>(1);
            var vehicles = new List<IVehicle>
            {
                new Car("ABC123", "Black", 4, FuelType.Gasoline),
                new Car("ABC124", "Red", 4, FuelType.Diesel),
                new Car("ABC125", "Blue", 4, FuelType.Diesel)
            };

            // Act
            var result = garage.Populate(vehicles);

            // Assert
            Assert.Equal(1, result.Parked);
            Assert.Equal(3, result.Total);
            Assert.Equal(new[] { "ABC124", "ABC125" }, result.Failures.Select(f => f.RegistrationNumber));
        }
    }
}

