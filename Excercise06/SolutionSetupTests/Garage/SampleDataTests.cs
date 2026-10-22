using SolutionSetup;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class SampleDataTests
    {
        [Fact]
        public void CreateSampleVehicles_ReturnsOnlyCars()
        {
            var vehicles = SampleData.CreateSampleVehicles();

            Assert.NotEmpty(vehicles);
            Assert.All(vehicles, vehicle => Assert.IsType<Car>(vehicle));
        }

        [Fact]
        public void CreateSampleVehicles_ReturnsUniqueRegistrationNumbers()
        {
            var vehicles = SampleData.CreateSampleVehicles();

            Assert.Equal(vehicles.Length, vehicles.Select(v => v.RegistrationNumber.ToUpperInvariant()).Distinct().Count());
        }
    }
}
