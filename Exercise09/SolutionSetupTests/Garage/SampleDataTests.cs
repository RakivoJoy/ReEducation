using SolutionSetup;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests;

    public class SampleDataTests
    {
        [Fact]
        public void CreateSampleVehicles_ReturnsOnlyCars()
        {
            // Act
            var vehicles = SampleData.CreateSampleVehicles();

            // Assert
            Assert.NotEmpty(vehicles);
            Assert.All(vehicles, vehicle => Assert.IsType<Car>(vehicle));
        }

        [Fact]
        public void CreateSampleVehicles_ReturnsUniqueRegistrationNumbers()
        {
            // Act
            var vehicles = SampleData.CreateSampleVehicles();

            // Assert
            Assert.Equal(vehicles.Count, vehicles.Select(v => v.RegistrationNumber.ToUpperInvariant()).Distinct().Count());
        }
    }

