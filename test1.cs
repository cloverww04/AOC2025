using System;
using System.IO;
using System.Numerics;

class Program
{
    static void Main()
    {
        BigInteger totalJoltage = 0;
        int b = 12; // 12 batteries

        foreach (var line in File.ReadLines("instructions.txt"))
        {
            string largest12 = MaxSubsequence(line, b);
            BigInteger joltage = BigInteger.Parse(largest12);
            totalJoltage += joltage;
        }

        Console.WriteLine($"Total output joltage: {totalJoltage}");
    }

    static string MaxSubsequence(string digits, int b)
    {
        var stack = new List<char>();
        int n = digits.Length;

        for (int i = 0; i < n; i++)
        {
            char c = digits[i];
            while (stack.Count > 0 && stack[^1] < c && stack.Count - 1 + (n - i) >= b)
            {
                stack.RemoveAt(stack.Count - 1);
            }
            stack.Add(c);
        }

        
        return new string([.. stack.GetRange(0, b)]);
    }
}
