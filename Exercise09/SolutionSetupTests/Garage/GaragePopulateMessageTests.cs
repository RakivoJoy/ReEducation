using SolutionSetup;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    // TODO: Phase 1 - Remove the Skip arguments once Populate() reports why each vehicle was not parked.
    public class GaragePopulateMessageTests
    {
        [Fact(Skip = "Phase 1: Populate() does not report failure reasons yet.")]
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

        [Fact(Skip = "Phase 1: Populate() does not report failure reasons yet.")]
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

        [Fact(Skip = "Phase 1: Populate() does not report failure reasons yet.")]
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
