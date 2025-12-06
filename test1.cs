using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var lines = File.ReadAllLines("instructions.txt")
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .ToArray();

        int numRows = lines.Length;
        int numCols = lines.Max(l => l.Length);

        for (int r = 0; r < numRows; r++)
            if (lines[r].Length < numCols)
                lines[r] = lines[r].PadRight(numCols, ' ');

        List<long> results = new List<long>();
        int col = numCols - 1;

        while (col >= 0)
        {
            if (lines.All(row => row[col] == ' '))
            {
                col--;
                continue;
            }

            int startCol = col;
            while (startCol > 0 && !lines.All(row => row[startCol - 1] == ' '))
                startCol--;

            List<long> numbers = new List<long>();
            for (int c = startCol; c <= col; c++)
            {
                string numStr = "";
                for (int r = 0; r < numRows - 1; r++)
                {
                    if (lines[r][c] != ' ')
                        numStr += lines[r][c];
                }

                if (!string.IsNullOrWhiteSpace(numStr))
                    numbers.Add(long.Parse(numStr));
            }

            char op = ' ';
            for (int c = startCol; c <= col; c++)
            {
                if (lines[numRows - 1][c] != ' ')
                {
                    op = lines[numRows - 1][c];
                    break;
                }
            }


            long result = numbers[0];
            for (int i = 1; i < numbers.Count; i++)
            {
                if (op == '+') result += numbers[i];
                else if (op == '*') result *= numbers[i];
            }

            results.Add(result);

            col = startCol - 2;
        }

        long grandTotal = results.Sum();
        Console.WriteLine("Grand Total = " + grandTotal);
    }
}
