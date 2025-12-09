using System;
using System.IO;
using System.Collections.Generic;

public struct Point2D
{
    public int x { get; set; }
    public int y { get; set; }

    public override string ToString() => $"({x}, {y})";
}

class Program
{
    static void Main()
    {
        string[] lines = File.ReadAllLines("instructions.txt");
        List<Point2D> points = new List<Point2D>();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            string[] parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
            {
                points.Add(new Point2D { x = x, y = y });
            }
        }

        long maxArea = 0;
        Point2D bestA = new Point2D();
        Point2D bestB = new Point2D();

        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                var p1 = points[i];
                var p2 = points[j];

                long width = Math.Abs(p2.x - p1.x) +1;
                long height = Math.Abs(p2.y - p1.y) +1;
                long area = width * height;

                if (area > maxArea)
                {
                    maxArea = area;
                    bestA = p1;
                    bestB = p2;
                }
            }
        }

        Console.WriteLine($"\nLargest area = {maxArea}");
        Console.WriteLine($"Formed by points {bestA} and {bestB}");
    }
}
