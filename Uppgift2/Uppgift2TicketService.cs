using System;
using System.Collections.Generic;

namespace Uppgift2
{
    // Service responsible for ticket price calculations and related helpers.
    public class Uppgift2TicketService
    {
        public int CalculatePriceForAge(int age)
        {
            try
            {
                if (age < 0)
                    throw new ArgumentException("Age cannot be negative.", nameof(age));

                if (age < 20)
                    return 80;
                else if (age > 64)
                    return 90;
                else
                    return 120;
            }
            catch (ArgumentException)
            {
                // Rethrow validation exceptions
                throw;
            }
        }

        public int CalculateTotalForGroup(IEnumerable<int> ages)
        {
            try
            {
                if (ages == null)
                    throw new ArgumentNullException(nameof(ages), "Age collection cannot be null.");

                int total = 0;
                foreach (int age in ages)
                {
                    try
                    {
                        total += CalculatePriceForAge(age);
                    }
                    catch (ArgumentException ex)
                    {
                        // Rethrow with context about which age caused the problem
                        throw new InvalidOperationException($"Failed to calculate price for age {age}.", ex);
                    }
                }
                return total;
            }
            catch (ArgumentNullException)
            {
                // Rethrow null reference exceptions
                throw;
            }
            catch (InvalidOperationException)
            {
                // Rethrow operation exceptions
                throw;
            }
        }
    }
}
