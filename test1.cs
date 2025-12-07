using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

class QuantumTachyon
{
    static void Main()
    {
        
        string[] lines = File.ReadAllLines("instructions.txt");

        // create a 2D grid of chars
        char[][] grid = lines.Select(l => l.ToCharArray()).ToArray();
        int rows = grid.Length;
        int cols = grid[0].Length;

        // find the S starting position
        int startR = -1, startC = -1;
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                if (grid[r][c] == 'S')
                {
                    startR = r;
                    startC = c;
                    break;
                }

        //  timeline count
        var memo = new Dictionary<(int r, int c), BigInteger>();

        BigInteger CountTimelines(int r, int c)
        {
            // if off grid exit and count as one completed timeline
            if (r < 0 || r >= rows || c < 0 || c >= cols)
                return BigInteger.One;

            var key = (r, c);
            if (memo.TryGetValue(key, out BigInteger cached))
                return cached;

            char cell = grid[r][c];

            BigInteger result;
            if (cell == '^')
            {
                // spawn left and right from same row
                result = CountTimelines(r, c - 1) + CountTimelines(r, c + 1);
            }
            else
            {
                // go down one row
                result = CountTimelines(r + 1, c);
            }

            memo[key] = result;
            return result;
        }

        BigInteger totalTimelines = CountTimelines(startR + 1, startC);

        Console.WriteLine(totalTimelines.ToString());
    }
}
