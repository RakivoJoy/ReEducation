using System;
using System.IO;

class JoinStrings
{
    public static void Main()
    {

        TextReader tIn = Console.In;
        TextWriter tOut = Console.Out;

        string start = tIn.ReadLine();
        string[] starts = start.Split(' ');
        int X = int.Parse(starts[0]);

        string[] lines = new string[X];

        for(int i = 0; i < X; i++)
        {
            lines[i] = tIn.ReadLine();
        }


        int lastIndex = -1;
        for (int i = 0; i < X-1; i++)
        {
            string operations = tIn.ReadLine();
            string[] ops = start.Split(' ');
            int a = int.Parse(ops[0]);
            int b = int.Parse(ops[1]);

            lines[a] = lines[a] + lines[b];
            lines[b] = "";

            lastIndex = a;
        }

        tOut.WriteLine(lines[lastIndex]);
    }
}