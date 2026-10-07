using System;
using System.Collections.Generic;

namespace Uppgift2
{
    // Service responsible for ticket price calculations and related helpers.
    public class Uppgift2TicketService
    {
        // Determine the single-person ticket price based on age.
        // Requirements:
        // - Convert input-age (string -> int) is performed by the caller or UI layer.
        // - If age < 20 -> "Youth price: 80kr"
        // - Else if age > 64 -> "Pensioner price: 90kr"
        // - Else -> "Standard price: 120kr"
        public int CalculatePriceForAge(int age)
        {
            // TODO: 
            throw new NotImplementedException();
        }

        // Calculate total cost for a group given a collection of ages.
        // Requirements:
        // - Ask how many people; caller will collect ages into a list/array and pass them in.
        // - For each age, determine individual price (reuse CalculatePriceForAge).
        // - Produce a summary containing: number of people and total cost (sum of individual prices).
        public int CalculateTotalForGroup(IEnumerable<int> ages)
        {
            // TODO:
            throw new NotImplementedException();
        }
    }
}
