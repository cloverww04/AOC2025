using System;
using System.IO;
using System.Numerics;

class Program
{
    static void Main()
    {
        string[] lines = File.ReadAllLines("instructions.txt");

        BigInteger totalInvalid = 0;
        int invalidCount = 0;

        foreach (string line in lines)
        {
            var ranges = line.Split(',');

            foreach (string range in ranges)
            {
                var parts = range.Split('-');
                if (parts.Length != 2) continue;

                long start = long.Parse(parts[0]);
                long end = long.Parse(parts[1]);

                for (long id = start; id <= end; id++)
                {
                    if (IsRepeatedPattern(id.ToString()))
                    {
                        invalidCount++;
                        totalInvalid += id;
                    }
                }
            }
        }

        Console.WriteLine($"Number of invalid IDs: {invalidCount}");
        Console.WriteLine($"Sum of invalid IDs: {totalInvalid}");
    }

    static bool IsRepeatedPattern(string s)
    {
        int len = s.Length;

        // Try every possible block size that divides the length
        for (int block = 1; block <= len / 2; block++)
        {
            if (len % block != 0) continue; // block size must divide length

            string piece = s.Substring(0, block);
            int repeats = len / block;

            bool ok = true;
            for (int i = 1; i < repeats; i++)
            {
                if (s.Substring(i * block, block) != piece)
                {
                    ok = false;
                    break;
                }
            }

            if (ok) return true; // pattern repeated at least twice
        }

        return false;
    }
}
