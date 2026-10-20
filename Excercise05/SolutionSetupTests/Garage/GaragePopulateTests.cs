using SolutionSetup;
using SolutionSetup.Vehicles;
using static SolutionSetupTests.TestVehicles;

namespace SolutionSetupTests
{
    public class GaragePopulateTests
    {
        [Fact]
        public void Populate_ParksAllVehicles_WhenCapacityAllows()
        {
            var garage = new Garage(3);

            var result = garage.Populate(SampleData.CreateSampleVehicles());

            Assert.True(result.Success);
            Assert.Equal(3, garage.ListAllVehicles().Length);
        }

        [Fact]
        public void Populate_ReturnsFailure_WhenCapacityExceeded()
        {
            var garage = new Garage(2);

            var result = garage.Populate(SampleData.CreateSampleVehicles());

            Assert.False(result.Success);
            Assert.Contains("Parked 2 of 3", result.Message);
            Assert.Equal(2, garage.ListAllVehicles().Length);
        }

        [Fact]
        public void Populate_ReturnsFailure_WhenRegistrationNumberDuplicated()
        {
            var garage = new Garage(3);
            var vehicles = new Vehicle[]
            {
                CreateCar("ABC123"),
                CreateCar("abc123")
            };

            var result = garage.Populate(vehicles);

            Assert.False(result.Success);
            Assert.Contains("Parked 1 of 2", result.Message);
        }

        [Fact]
        public void Populate_ThrowsForNull()
        {
            var garage = new Garage(1);

            Assert.Throws<ArgumentNullException>(() => garage.Populate(null!));
        }
    }
}
