using System;
using System.Collections.Generic;
using Xunit;
using Uppgift2;

namespace Uppgift2.Tests
{
    // Test stubs for Uppgift2TicketService following a TDD-style:
    // Each test describes the expected behavior but is intentionally left unimplemented.
    // When implementing the exercise the tests should be completed and the service code
    // should be updated until all tests pass.
    public class Uppgift2TicketServiceTests
    {
        [Fact]
        public void CalculatePriceForAge_Under20_Returns80()
        {
            // Arrange
            // var service = new Uppgift2TicketService();
            // Act
            // var price = service.CalculatePriceForAge(19);
            // Assert
            // Assert.Equal(80, price);
            throw new NotImplementedException();
        }

        [Fact]
        public void CalculatePriceForAge_Exactly20_ReturnsStandardPrice()
        {
            // Arrange
            // var service = new Uppgift2TicketService();
            // Act
            // var price = service.CalculatePriceForAge(20);
            // Assert
            // Assert.Equal(120, price);
            throw new NotImplementedException();
        }

        [Fact]
        public void CalculatePriceForAge_Over64_Returns90()
        {
            // Arrange
            // var service = new Uppgift2TicketService();
            // Act
            // var price = service.CalculatePriceForAge(65);
            // Assert
            // Assert.Equal(90, price);
            throw new NotImplementedException();
        }

        [Fact]
        public void CalculateTotalForGroup_MixedAges_ReturnsCorrectSummary()
        {
            // Arrange
            // var service = new Uppgift2TicketService();
            // var ages = new List<int> { 10, 30, 70 }; // expected prices: 80 + 120 + 90 = 290
            // Act
            // var total = service.CalculateTotalForGroup(ages);
            // Assert
            // Assert.Equal(290, total);
            throw new NotImplementedException();
        }

        [Fact]
        public void CalculateTotalForGroup_EmptyGroup_ReturnsZero()
        {
            // Arrange
            // var service = new Uppgift2TicketService();
            // var ages = new List<int>();
            // Act
            // var total = service.CalculateTotalForGroup(ages);
            // Assert
            // Assert.Equal(0, total);
            throw new NotImplementedException();
        }
    }
}
