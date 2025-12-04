using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        int[] dr = [-1, -1, -1, 0, 0, 1, 1, 1];
        int[] dc = [-1,  0,  1,-1, 1,-1, 0, 1];

        var lines = File.ReadAllLines("instructions.txt");
        List<char[]> grid = [];

        foreach (var line in lines)
            grid.Add(line.ToCharArray());

        int totalRemoved = 0;

        while (true)
        {
            List<(int r, int c)> toRemove = [];

            for (int r = 0; r < grid.Count; r++)
            {
                for (int c = 0; c < grid[r].Length; c++)
                {
                    if (grid[r][c] != '@')
                        continue;

                    int neigh = 0;

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

                    if (neigh < 4)
                        toRemove.Add((r, c));
                }
            }

            if (toRemove.Count == 0)
                break;

            foreach (var (r, c) in toRemove)
                grid[r][c] = '.';

            totalRemoved += toRemove.Count;
        }

        Console.WriteLine("Total removed: " + totalRemoved);
    }
}
