using System;
using System.IO;
using System.Linq;

class Problems99
{
    public static void Main()
    {

        TextReader tIn = Console.In;
        TextWriter tOut = Console.Out;

        string start = tIn.ReadLine();
        string[] starts = start.Split(' ');

        int X = int.Parse(starts[0]);

        if (X < 100)
        {
            tOut.WriteLine(99);
            return;
        }

        int xUpper = X;
        int xLower = X;

        while (xUpper % 100 != 99 && xLower % 100 != 99)
        {
            xUpper++;
            xLower--;
        }

        if (xUpper % 100 == 99)
        {
            tOut.WriteLine(xUpper);
        }
        else
        {
            tOut.WriteLine(xLower);
        }
        // Test
    }
}