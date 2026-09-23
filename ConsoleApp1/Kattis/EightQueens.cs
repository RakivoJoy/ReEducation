using System;
using System.IO;

class EightQueens
{

    static char[,] board;

    static TextReader tIn = Console.In;
    static TextWriter tOut = Console.Out;

    public static void Main()
    {


        board = new char[8,8];

        // Read the board from input
        for (int i = 0; i < 8; i++)
        {
            string s = tIn.ReadLine();
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = s[j];
            }
        }


        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                char c = board[i, j];
                if(c.Equals('*'))
                {
                    if(IsInLineOfQueen(i, j))
                    {
                        tOut.WriteLine("invalid");
                        return; // Early exit if a queen can attack another
                    }
                }
            }
        }

        tOut.WriteLine("valid");
        return;
    }

    private static bool IsInLineOfQueen(int i, int j)
    {
        // Check row
        for (int k = 0; k < 8; k++)
        {
            if (board[i, k] == '*' && k != j)
            {
                return true;
            }
        }

        // Check column
        for (int k = 0; k < 8; k++)
        {
            if (board[k, j] == '*' && k != i)
            {
                return true;
            }
        }

        // Check diagonals
        for (int k = -7; k <= 7; k++)
        {
            if (i + k >= 0 && i + k < 8 && j + k >= 0 && j + k < 8)
            {
                if (board[i + k, j + k] == '*' && !(i + k == i && j + k == j))
                {
                    return true;
                }
            }
        }

        return false;
    }
}