using System;
using System.Collections.Generic;
using Xunit;
using Uppgift2;

namespace Uppgift2.Tests
{
    public class Uppgift2TicketServiceTests
    {
        [Fact]
        public void CalculatePriceForAge_Under20_Returns80()
        {
            var service = new Uppgift2TicketService();
            var price = service.CalculatePriceForAge(19);
            Assert.Equal(80, price);
        }

        [Fact]
        public void CalculatePriceForAge_Age0_Returns80()
        {
            var service = new Uppgift2TicketService();
            var price = service.CalculatePriceForAge(0);
            Assert.Equal(80, price);
        }

        [Fact]
        public void CalculatePriceForAge_Exactly20_ReturnsStandardPrice()
        {
            var service = new Uppgift2TicketService();
            var price = service.CalculatePriceForAge(20);
            Assert.Equal(120, price);
        }

        [Fact]
        public void CalculatePriceForAge_Age30_ReturnsStandardPrice()
        {
            var service = new Uppgift2TicketService();
            var price = service.CalculatePriceForAge(30);
            Assert.Equal(120, price);
        }

        [Fact]
        public void CalculatePriceForAge_Exactly64_ReturnsStandardPrice()
        {
            var service = new Uppgift2TicketService();
            var price = service.CalculatePriceForAge(64);
            Assert.Equal(120, price);
        }

        [Fact]
        public void CalculatePriceForAge_Over64_Returns90()
        {
            var service = new Uppgift2TicketService();
            var price = service.CalculatePriceForAge(65);
            Assert.Equal(90, price);
        }

        [Fact]
        public void CalculatePriceForAge_Age100_Returns90()
        {
            var service = new Uppgift2TicketService();
            var price = service.CalculatePriceForAge(100);
            Assert.Equal(90, price);
        }

        [Fact]
        public void CalculateTotalForGroup_MixedAges_ReturnsCorrectSummary()
        {
            var service = new Uppgift2TicketService();
            var ages = new List<int> { 10, 30, 70 }; // expected prices: 80 + 120 + 90 = 290
            var total = service.CalculateTotalForGroup(ages);
            Assert.Equal(290, total);
        }

        [Fact]
        public void CalculateTotalForGroup_EmptyGroup_ReturnsZero()
        {
            var service = new Uppgift2TicketService();
            var ages = new List<int>();
            var total = service.CalculateTotalForGroup(ages);
            Assert.Equal(0, total);
        }

        [Fact]
        public void CalculateTotalForGroup_SingleYouth_Returns80()
        {
            var service = new Uppgift2TicketService();
            var ages = new List<int> { 15 };
            var total = service.CalculateTotalForGroup(ages);
            Assert.Equal(80, total);
        }

        [Fact]
        public void CalculateTotalForGroup_SingleStandard_Returns120()
        {
            var service = new Uppgift2TicketService();
            var ages = new List<int> { 40 };
            var total = service.CalculateTotalForGroup(ages);
            Assert.Equal(120, total);
        }

        [Fact]
        public void CalculateTotalForGroup_SinglePensioner_Returns90()
        {
            var service = new Uppgift2TicketService();
            var ages = new List<int> { 75 };
            var total = service.CalculateTotalForGroup(ages);
            Assert.Equal(90, total);
        }

        [Fact]
        public void CalculateTotalForGroup_AllYouth_ReturnsCorrectSum()
        {
            var service = new Uppgift2TicketService();
            var ages = new List<int> { 5, 10, 18, 19 };
            var total = service.CalculateTotalForGroup(ages);
            Assert.Equal(320, total); // 4 * 80
        }

        [Fact]
        public void CalculateTotalForGroup_AllPensioners_ReturnsCorrectSum()
        {
            var service = new Uppgift2TicketService();
            var ages = new List<int> { 65, 70, 80, 90 };
            var total = service.CalculateTotalForGroup(ages);
            Assert.Equal(360, total); // 4 * 90
        }
    }
}
