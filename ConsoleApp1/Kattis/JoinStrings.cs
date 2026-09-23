using System;
using System.IO;
using System.Text;

class JoinStrings
{
    static void Main()
    {
        // Streams for better I/O performance
        var input = new StreamReader(Console.OpenStandardInput(), Encoding.ASCII, false, 1 << 20);
        var output = new StreamWriter(Console.OpenStandardOutput(), Encoding.ASCII, 1 << 20);
        Console.SetOut(output);

        int n = int.Parse(input.ReadLine().Trim());

        // Setting up head and tails to trace strings.
        string[] s = new string[n + 1];
        int[] head = new int[n + 1];
        int[] tail = new int[n + 1];
        int[] next = new int[n + 1]; // 0 = end of chain
        bool[] alive = new bool[n + 1]; // Overwritten strings are marked as false.

        for (int i = 1; i <= n; i++)
        {
            s[i] = input.ReadLine().Trim();
            head[i] = i;
            tail[i] = i;
            next[i] = 0;
            alive[i] = true;
        }

        // Operations
        int ops = n - 1; 
        for (int op = 0; op < ops; op++)
        {
            string line = input.ReadLine();
            int sp = line.IndexOf(' ');
            int x = int.Parse(line.Substring(0, sp).Trim());
            int y = int.Parse(line.Substring(sp + 1).Trim());

            // a[x] = a[x] + a[y]; a[y] = ""  --> Quick O(1) pointer relinking
            next[tail[x]] = head[y];
            tail[x] = tail[y];
            alive[y] = false;
        }

        // Finding the answer by traversal
        int answer = -1;
        for (int i = 1; i <= n; i++)
        {
            if (alive[i]) { answer = i; break; }
        }

        // Output the final string by traversing the linked list of strings starting from head[ans].
        var sb = new StringBuilder();
        int current = head[answer];
        while (current != 0)
        {
            sb.Append(s[current]);
            current = next[current];
        }

        output.WriteLine(sb.ToString());
        output.Flush();
    }
}