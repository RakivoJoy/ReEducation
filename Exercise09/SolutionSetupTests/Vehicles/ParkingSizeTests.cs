using SolutionSetup;
using SolutionSetup.UI;
using SolutionSetup.Vehicles;

namespace SolutionSetupTests
{
    public class ParkingSizeTests
    {
        [Theory]
        [InlineData(VehicleType.Motorcycle, 1, 10)]
        [InlineData(VehicleType.Car, 5, 30)]
        [InlineData(VehicleType.Bus, 20, 100)]
        [InlineData(VehicleType.Truck, 15, 80)]
        [InlineData(VehicleType.Boat, 10, 400)]
        [InlineData(VehicleType.Airplane, 50, 400)]
        public void Limits_AreDefinedPerType(VehicleType type, int min, int max)
        {
            // Arrange
            // Act
            var limits = ParkingSizeLimits.For(type);

            // Assert
            Assert.Equal((min, max), limits);
        }

        [Fact]
        public void Limits_AreInsideOverallRange_ForEveryType()
        {
            // Arrange
            foreach (VehicleType type in Enum.GetValues<VehicleType>())
            {
                // Act
                var (min, max) = ParkingSizeLimits.For(type);

                // Assert
                Assert.InRange(min, ParkingSizeLimits.Min, ParkingSizeLimits.Max);
                Assert.InRange(max, ParkingSizeLimits.Min, ParkingSizeLimits.Max);
            }
        }

        [Theory]
        [InlineData(4)]
        [InlineData(31)]
        public void Car_RejectsParkingSizeOutsideLimits(int size)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Car("ABC123", "Black", 4, FuelType.Diesel, size));
        }

        [Theory]
        [InlineData(5)]
        [InlineData(30)]
        public void Car_AcceptsParkingSizeAtLimits(int size)
        {
            // Arrange
            var car = new Car("ABC123", "Black", 4, FuelType.Diesel, size);

            // Act
            int actual = car.ParkingSize;

            // Assert
            Assert.Equal(size, actual);
        }

        [Fact]
        public void Vehicle_DefaultsToSmallestSizeForType()
        {
            // Arrange
            var motorcycle = new Motorcycle("ABC123", "Black", 2, 600);

            // Act
            int size = motorcycle.ParkingSize;

            // Assert
            Assert.Equal(1, size);
        }

        [Theory]
        [InlineData("40", 40, 40)]
        [InlineData("10-30", 10, 30)]
        public void IntRange_ParsesSingleValueAndRange(string text, int min, int max)
        {
            // Arrange
            var expected = new IntRange(min, max);

            // Act
            bool parsed = IntRange.TryParse(text, out IntRange? range);

            // Assert
            Assert.True(parsed);
            Assert.Equal(expected, range);
        }

        [Theory]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("30-10")]
        [InlineData("-5")]
        [InlineData("1-2-3")]
        public void IntRange_RejectsInvalidText(string text)
        {
            // Arrange
            // Act
            bool parsed = IntRange.TryParse(text, out _);

            // Assert
            Assert.False(parsed);
        }

        [Fact]
        public void SearchByProperties_FiltersBySizeRange()
        {
            // Arrange
            var garage = new Garage<IVehicle>(3);
            garage.AddVehicle(new Car("AAA111", "Black", 4, FuelType.Diesel, 10));
            garage.AddVehicle(new Car("BBB222", "Black", 4, FuelType.Diesel, 20));
            garage.AddVehicle(new Car("CCC333", "Black", 4, FuelType.Diesel, 30));
            var criteria = new SearchCriteria();
            Assert.True(SearchCriteriaParser.TrySetFilter(criteria, "size", "15-30", out _));

            // Act
            var results = garage.SearchByProperties(criteria);

            // Assert
            Assert.Equal(["BBB222", "CCC333"], results.Select(v => v.RegistrationNumber).Order());
        }

        [Fact]
        public void SearchByProperties_SingleSizeMatchesExactly()
        {
            // Arrange
            var garage = new Garage<IVehicle>(2);
            garage.AddVehicle(new Car("AAA111", "Black", 4, FuelType.Diesel, 10));
            garage.AddVehicle(new Car("BBB222", "Black", 4, FuelType.Diesel, 11));

            // Act
            var results = garage.SearchByProperties(new SearchCriteria { ParkingSize = new IntRange(10, 10) });

            // Assert
            Assert.Single(results);
        }

        [Fact]
        public void TrySetFilter_RejectsInvalidSize()
        {
            var criteria = new SearchCriteria();

            // Act
            bool isSet = SearchCriteriaParser.TrySetFilter(criteria, "size", "30-10", out string error);

            // Assert
            Assert.False(isSet);
            Assert.Contains("Invalid parking size '30-10'", error);
        }

        [Fact]
        public void ParkVehicle_PromptsForSizeWithTypeLimits_AndRetriesOutOfRange()
        {
            // Arrange
            var handler = new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider());
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\n99\n12\nGasoline\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            // Act
            ui.ParkVehicle();

            // Assert
            string text = output.ToString();
            Assert.Contains("Enter parking size (5-30):", text);
            Assert.Contains("Invalid parking size '99'", text);
            Assert.Equal(12, handler.FindVehicle("ABC123")!.ParkingSize);
        }

        [Fact]
        public void SearchVehicles_FindsBySizeRange()
        {
            // Arrange
            var handler = new GarageHandler(new InMemoryGarageStorage(), new SampleDataProvider());
            using var output = new StringWriter();
            using var input = new StringReader(
                "5\n" +
                "3\nABC123\nBlack\n4\n10\nGasoline\n" +
                "3\nDEF456\nBlack\n4\n25\nGasoline\n" +
                "size=20-30\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();
            ui.ParkVehicle();
            ui.ParkVehicle();

            // Act
            ui.SearchVehicles();

            // Assert
            string text = output.ToString();
            Assert.Contains("DEF456", text.Substring(text.LastIndexOf("Enter criteria", StringComparison.Ordinal)));
            Assert.DoesNotContain("ABC123", text.Substring(text.LastIndexOf("Enter criteria", StringComparison.Ordinal)));
        }
    }
}
