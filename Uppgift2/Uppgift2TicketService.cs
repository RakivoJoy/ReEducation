using System;
using System.Collections.Generic;

namespace Uppgift2
{
    // Service responsible for ticket price calculations and related helpers.
    public class Uppgift2TicketService
    {
        public int CalculatePriceForAge(int age)
        {
            if (age < 20)
                return 80;
            else if (age > 64)
                return 90;
            else
                return 120;
        }

        public int CalculateTotalForGroup(IEnumerable<int> ages)
        {
            int total = 0;
            foreach (int age in ages)
            {
                total += CalculatePriceForAge(age);
            }
            return total;
        }
    }
}
