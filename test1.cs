using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Directions: 8 neighbors
        int[] dr = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dc = { -1,  0,  1,-1, 1,-1, 0, 1 };

        // Load input into a mutable grid (char arrays!)
        var lines = File.ReadAllLines("instructions.txt");
        List<char[]> grid = new List<char[]>();

        foreach (var line in lines)
            grid.Add(line.ToCharArray());

        int totalRemoved = 0;

        while (true)
        {
            List<(int r, int c)> toRemove = new();

            // Scan grid to find accessible rolls
            for (int r = 0; r < grid.Count; r++)
            {
                for (int c = 0; c < grid[r].Length; c++)
                {
                    if (grid[r][c] != '@')
                        continue;

                    int neigh = 0;

                    // Count neighbors
                    for (int k = 0; k < 8; k++)
                    {
                        int nr = r + dr[k];
                        int nc = c + dc[k];

                        if (nr >= 0 && nr < grid.Count &&
                            nc >= 0 && nc < grid[nr].Length &&
                            grid[nr][nc] == '@')
                        {
                            neigh++;
                        }
                    }

                    // Fewer than 4 means accessible
                    if (neigh < 4)
                        toRemove.Add((r, c));
                }
            }

            // Stop if nothing left to remove
            if (toRemove.Count == 0)
                break;

            // Remove all found '@' this round
            foreach (var (r, c) in toRemove)
                grid[r][c] = '.';

            totalRemoved += toRemove.Count;
        }

        Console.WriteLine("Total removed: " + totalRemoved);
    }
}
