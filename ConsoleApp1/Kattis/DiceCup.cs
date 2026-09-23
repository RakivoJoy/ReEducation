using System;
using System.IO;
using System.Linq;

class DiceCup
{
    public static void Main()
    {

        TextReader tIn = Console.In;
        TextWriter tOut = Console.Out;

        string start = tIn.ReadLine();
        string[] starts = start.Split(' ');

        int X = int.Parse(starts[0]);
        int Y = int.Parse(starts[1]);

        // Create an array to hold the counts of sums
        int[] sums = new int[X +Y+1];
        for (int i = 1; i <= X; i++)
        {
            for (int j = 1; j <= Y; j++)
            {
                sums[i + j]++; // Increment the count for the sum of the two dice
            }
        }

        int max = sums.Max();
        for(int i = 2; i < sums.Length; i++)
        {
            if (sums[i] == max)
            {
                tOut.WriteLine(i);
            }
        }
    }
}