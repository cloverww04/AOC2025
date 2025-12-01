using System;
using System.IO;

class Program
{
    static void Main()
    {
        string[] lines = File.ReadAllLines("instructions.txt");

        int dial = 50;
        int zeroCount = 0;

        foreach (string line in lines)
        {
            char direction = line[0];
            int move = int.Parse(line[1..]);

            if (direction == 'R')
            {
                for (int i = 0; i < move; i++)
                {
                    dial = (dial + 1) % 100;
                    if (dial == 0)
                        zeroCount++;
                }
            }
            else if (direction == 'L')
            {
                for (int i = 0; i < move; i++)
                {
                    dial = (dial - 1 + 100) % 100;
                    if (dial == 0)
                        zeroCount++;
                }
            }
        }

        Console.WriteLine($"Final dial position: {dial}");
        Console.WriteLine($"Password (times dial hit 0): {zeroCount}");
    }
}
