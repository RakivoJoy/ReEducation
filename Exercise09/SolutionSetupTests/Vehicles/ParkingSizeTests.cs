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
            Assert.Equal((min, max), ParkingSizeLimits.For(type));
        }

        [Fact]
        public void Limits_AreInsideOverallRange_ForEveryType()
        {
            foreach (VehicleType type in Enum.GetValues<VehicleType>())
            {
                var (min, max) = ParkingSizeLimits.For(type);
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
            Assert.Equal(size, new Car("ABC123", "Black", 4, FuelType.Diesel, size).ParkingSize);
        }

        [Fact]
        public void Vehicle_DefaultsToSmallestSizeForType()
        {
            Assert.Equal(1, new Motorcycle("ABC123", "Black", 2, 600).ParkingSize);
        }

        [Theory]
        [InlineData("40", 40, 40)]
        [InlineData("10-30", 10, 30)]
        public void IntRange_ParsesSingleValueAndRange(string text, int min, int max)
        {
            Assert.True(IntRange.TryParse(text, out IntRange? range));
            Assert.Equal(new IntRange(min, max), range);
        }

        [Theory]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("30-10")]
        [InlineData("-5")]
        [InlineData("1-2-3")]
        public void IntRange_RejectsInvalidText(string text)
        {
            Assert.False(IntRange.TryParse(text, out _));
        }

        [Fact]
        public void SearchByProperties_FiltersBySizeRange()
        {
            var garage = new Garage<IVehicle>(3);
            garage.AddVehicle(new Car("AAA111", "Black", 4, FuelType.Diesel, 10));
            garage.AddVehicle(new Car("BBB222", "Black", 4, FuelType.Diesel, 20));
            garage.AddVehicle(new Car("CCC333", "Black", 4, FuelType.Diesel, 30));
            var criteria = new SearchCriteria();
            Assert.True(criteria.TrySetFilter("size", "15-30", out _));

            var results = garage.SearchByProperties(criteria);

            Assert.Equal(["BBB222", "CCC333"], results.Select(v => v.RegistrationNumber).Order());
        }

        [Fact]
        public void SearchByProperties_SingleSizeMatchesExactly()
        {
            var garage = new Garage<IVehicle>(2);
            garage.AddVehicle(new Car("AAA111", "Black", 4, FuelType.Diesel, 10));
            garage.AddVehicle(new Car("BBB222", "Black", 4, FuelType.Diesel, 11));

            var results = garage.SearchByProperties(new SearchCriteria { ParkingSize = new IntRange(10, 10) });

            Assert.Single(results);
        }

        [Fact]
        public void TrySetFilter_RejectsInvalidSize()
        {
            var criteria = new SearchCriteria();

            Assert.False(criteria.TrySetFilter("size", "30-10", out string error));
            Assert.Contains("Invalid parking size '30-10'", error);
        }

        [Fact]
        public void ParkVehicle_PromptsForSizeWithTypeLimits_AndRetriesOutOfRange()
        {
            var handler = new GarageHandler();
            using var output = new StringWriter();
            using var input = new StringReader("5\n3\nABC123\nBlack\n4\n99\n12\nGasoline\n");
            var ui = new ConsoleUI(input, output, handler);
            ui.CreateGarage();

            ui.ParkVehicle();

            string text = output.ToString();
            Assert.Contains("Enter parking size (5-30):", text);
            Assert.Contains("Invalid parking size '99'", text);
            Assert.Equal(12, handler.FindVehicle("ABC123")!.ParkingSize);
        }

        [Fact]
        public void SearchVehicles_FindsBySizeRange()
        {
            var handler = new GarageHandler();
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

            ui.SearchVehicles();

            string text = output.ToString();
            Assert.Contains("DEF456", text.Substring(text.LastIndexOf("Enter criteria", StringComparison.Ordinal)));
            Assert.DoesNotContain("ABC123", text.Substring(text.LastIndexOf("Enter criteria", StringComparison.Ordinal)));
        }
    }
}
