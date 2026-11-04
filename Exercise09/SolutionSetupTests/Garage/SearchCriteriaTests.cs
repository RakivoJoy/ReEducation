using SolutionSetup;
using SolutionSetup.UI;
using SolutionSetup.Vehicles;
using static SolutionSetupTests.TestVehicles;

namespace SolutionSetupTests
{
    public class SearchCriteriaTests
    {
        [Fact]
        public void PropertyNames_IncludesFuel()
        {
            Assert.Contains("fuel", SearchCriteria.PropertyNames);
        }

        [Fact]
        public void TrySetFilter_IsCaseInsensitiveForNameAndValue()
        {
            var criteria = new SearchCriteria();

            bool ok = SearchCriteriaParser.TrySetFilter(criteria, "FUEL", "diesel", out string error);

            Assert.True(ok);
            Assert.Equal(string.Empty, error);
            Assert.Equal(FuelType.Diesel, criteria.FuelType);
        }

        [Theory]
        [InlineData("fuel", "water", "Invalid fuel type 'water'")]
        [InlineData("fuel", "7", "Invalid fuel type '7'")]
        [InlineData("wheels", "many", "Invalid number of wheels")]
        [InlineData("weight", "big", "Unknown search property 'weight'")]
        public void TrySetFilter_RejectsInvalidInput(string property, string text, string expected)
        {
            var criteria = new SearchCriteria();

            bool ok = SearchCriteriaParser.TrySetFilter(criteria, property, text, out string error);

            Assert.False(ok);
            Assert.Contains(expected, error);
        }

        [Fact]
        public void Matches_FuelOnlyMatchesCarsWithThatFuel()
        {
            var criteria = new SearchCriteria { FuelType = FuelType.Diesel };

            Assert.True(criteria.Matches(new Car("AAA111", "Black", 4, FuelType.Diesel)));
            Assert.False(criteria.Matches(new Car("BBB222", "Black", 4, FuelType.Gasoline)));
            Assert.False(criteria.Matches(new Motorcycle("CCC333", "Black", 2, 600)));
        }

        [Fact]
        public void SettingTypedPropertyToNull_RemovesFilter()
        {
            var criteria = new SearchCriteria { Color = "Black" };

            criteria.Color = null;

            Assert.True(criteria.Matches(new Car("AAA111", "Red", 4, FuelType.Diesel)));
        }

        [Fact]
        public void SearchByProperties_FiltersByFuelAndColor()
        {
            var garage = new Garage<IVehicle>(4);
            garage.AddVehicle(new Car("AAA111", "Black", 4, FuelType.Diesel));
            garage.AddVehicle(new Car("BBB222", "Black", 4, FuelType.Gasoline));
            garage.AddVehicle(new Car("CCC333", "Red", 4, FuelType.Diesel));

            var criteria = new SearchCriteria { Color = "black" };
            Assert.True(SearchCriteriaParser.TrySetFilter(criteria, "fuel", "Diesel", out _));

            var results = garage.SearchByProperties(criteria);

            Assert.Single(results);
            Assert.Equal("AAA111", results[0].RegistrationNumber);
        }
    }
}
