using SolutionSetup;
using SolutionSetup.Vehicles;
using static SolutionSetupTests.TestVehicles;

namespace SolutionSetupTests
{
    public class SearchPropertiesTests
    {
        [Fact]
        public void Find_IsCaseInsensitive()
        {
            Assert.Equal("color", SearchProperties.Find("COLOR")?.Name);
        }

        [Fact]
        public void Find_ReturnsNullForUnknownProperty()
        {
            Assert.Null(SearchProperties.Find("size"));
        }

        [Fact]
        public void Parse_ReturnsNullForInvalidWheels()
        {
            SearchProperty wheels = SearchProperties.Find("wheels")!;

            Assert.Null(wheels.Parse.Invoke("many"));
        }

        [Fact]
        public void SetFilter_NullRemovesFilter()
        {
            var criteria = new SearchCriteria { Color = "Black" };

            criteria.SetFilter("color", null);

            Assert.Empty(criteria.Filters);
        }

        [Fact]
        public void SearchByProperties_UsesFiltersSetByName()
        {
            var garage = new Garage<IVehicle>(2);
            garage.AddVehicle(CreateCar("ABC123", "Black"));
            garage.AddVehicle(CreateCar("DEF456", "Red"));

            var criteria = new SearchCriteria();
            criteria.SetFilter("color", "black");

            var results = garage.SearchByProperties(criteria);

            Assert.Single(results);
            Assert.Equal("ABC123", results[0].RegistrationNumber);
        }
    }
}
