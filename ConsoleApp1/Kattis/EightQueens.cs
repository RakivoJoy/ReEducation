using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;

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

        int queenCount = 0;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                char c = board[i, j];
                if(c.Equals('*'))
                {
                    queenCount++;
                    if (IsInLineOfQueen(i, j))
                    {
                        tOut.WriteLine("invalid");
                        return; // Early exit if a queen can attack another
                    }
                }
            }
        }

        if(queenCount != 8)
        {
            tOut.WriteLine("invalid");
            return;
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
        int row = i, col = j;

        while(row >= 0 && col >= 0)
        {
            if (board[row, col] == '*' && (row != i || col != j))
            {
                return true;
            }
            row--;
            col--;

        }

        row = i;
        col = j;
        while (row < 8 && col < 8)
        {
            if (board[row, col] == '*' && (row != i || col != j))
            {
                return true;
            }
            row++;
            col++;

        }

        row = i;
        col = j;
        while (row >= 0 && col < 8)
        {
            if (board[row, col] == '*' && (row != i || col != j))
            {
                return true;
            }
            row--;
            col++;

        }

        row = i;
        col = j;
        while (row < 8 && col >= 0)
        {
            if (board[row, col] == '*' && (row != i || col != j))
            {
                return true;
            }
            row++;
            col--;

        }

        return false;
    }

   
}