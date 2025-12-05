using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

class RangeMerger
{
    public static void Main()
    {
        string filePath = "instructions.txt";
        
        List<(long Min, long Max)> freshRanges = new List<(long Min, long Max)>(); 

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine))
                {
                    break;
                }

                try
                {
                    string[] parts = trimmedLine.Split('-');
                    if (parts.Length == 2)
                    {
                        long min = long.Parse(parts[0].Trim());
                        long max = long.Parse(parts[1].Trim());
                        freshRanges.Add((min, max));
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error: Skipping invalid range line '{trimmedLine}'. Details: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            return;
        }

        if (freshRanges.Count == 0)
        {
            Console.WriteLine("No fresh ranges found in the file.");
            return;
        }


        freshRanges.Sort((a, b) => a.Min.CompareTo(b.Min));

        List<(long Min, long Max)> daIDs = new List<(long Min, long Max)>();

        long currentMin = freshRanges[0].Min;
        long currentMax = freshRanges[0].Max;

        for (int i = 1; i < freshRanges.Count; i++)
        {
            long nextMin = freshRanges[i].Min;
            long nextMax = freshRanges[i].Max;

            if (nextMin <= currentMax + 1)
            {
                currentMax = Math.Max(currentMax, nextMax);
            }
            else
            {
                daIDs.Add((currentMin, currentMax));

                currentMin = nextMin;
                currentMax = nextMax;
            }
        }

        daIDs.Add((currentMin, currentMax));

        BigInteger totalFreshCount = BigInteger.Zero;
        
        foreach (var range in daIDs)
        {

            BigInteger length = new BigInteger(range.Max - range.Min + 1);
            totalFreshCount += length;
        }

        Console.WriteLine($"Total Initial Ranges: {freshRanges.Count}");
        Console.WriteLine($"Total Merged Ranges: {daIDs.Count}");
        Console.WriteLine($"Total **Fresh** Ingredient IDs found (covered length): {totalFreshCount}");
    }
}