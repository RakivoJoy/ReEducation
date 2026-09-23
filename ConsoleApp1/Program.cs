using System;
using System.IO;

class InTest
{
    public static void Main()
    {

        TextReader tIn = Console.In;
        TextWriter tOut = Console.Out;


        string start = tIn.ReadLine();

        string[] starts = start.Split(' ');

        int amount = int.Parse(starts[0]);
        int id = int.Parse(starts[1]);

        string bags = tIn.ReadLine();


        string[] bag = bags.Split(' ');

        int[] bagArray = new int[amount];

        for (int i = 0; i < amount; i++)
        {
            bagArray[i] = int.Parse(bag[i]);

            if(bagArray[i] == id)
            {
                if(i == 0)
                {
                    tOut.WriteLine("fyrst");
                }
                else if (i == 1)
                {
                    tOut.WriteLine("naestfyrst");
                }
                else
                {
                    tOut.WriteLine(i+1 + " fyrst");
                }
                return;
            }
        }
    }
}