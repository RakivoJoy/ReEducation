using SolutionSetup;
using SolutionSetup.Vehicles;

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
            Assert.False(result.Success);
            Assert.Contains("ABC124", result.Message);
            Assert.Contains("The garage is full.", result.Message);
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
            Assert.False(result.Success);
            Assert.Contains("already parked", result.Message);
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
            Assert.Contains("Parked 1 of 3", result.Message);
            Assert.Contains("ABC124", result.Message);
            Assert.Contains("ABC125", result.Message);
        }
    }
}

