using System;
using System.IO;

class FizzBuzz
{
    public static void Main()
    {

        TextReader tIn = Console.In;
        TextWriter tOut = Console.Out;

        string start = tIn.ReadLine();
        string[] starts = start.Split(' ');

        int X = int.Parse(starts[0]);
        int Y = int.Parse(starts[1]);
        int Z = int.Parse(starts[2]);

        for(int i = 1; i <= Z; i++)
        {
            if (i % X == 0 && i % Y == 0)
            {
                tOut.WriteLine("FizzBuzz");
            }
            else if (i % X == 0)
            {
                tOut.WriteLine("Fizz");
            }
            else if (i % Y == 0)
            {
                tOut.WriteLine("Buzz");
            }
            else
            {
                tOut.WriteLine(i);
            }
        }
    }
}