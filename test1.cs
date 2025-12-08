using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


public struct Point3D
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Point3D(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}

// Represents a potential connection between two junction boxes
public class Edge
{
    public int U { get; } // Index of the first point
    public int V { get; } // Index of the second point
    public long SquaredDistance { get; } 

    public Edge(int u, int v, long distance)
    {
        U = u;
        V = v;
        SquaredDistance = distance;
    }
}

// Disjoint Set Union (DSU) or Union-Find structure
public class DSU
{
    private int[] Parent; 
    private int[] Size;   

    public DSU(int n)
    {
        Parent = new int[n];
        Size = new int[n];
        for (int i = 0; i < n; i++)
        {
            Parent[i] = i; 
            Size[i] = 1;   
        }
    }

    public int Find(int i)
    {
        if (Parent[i] == i)
            return i;
        return Parent[i] = Find(Parent[i]);
    }

    // Merges the sets containing i and j (returns true if a merge occurred)
    public bool Union(int i, int j)
    {
        int rootI = Find(i);
        int rootJ = Find(j);

        if (rootI != rootJ)
        {
            if (Size[rootI] < Size[rootJ])
            {
                Parent[rootI] = rootJ;
                Size[rootJ] += Size[rootI];
            }
            else
            {
                Parent[rootJ] = rootI;
                Size[rootI] += Size[rootJ];
            }
            return true;
        }
        return false; // Already in the same circuit
    }

    public List<int> GetCircuitSizes()
    {
        List<int> sizes = new List<int>();
        for (int i = 0; i < Parent.Length; i++)
        {
            if (Parent[i] == i)
            {
                sizes.Add(Size[i]);
            }
        }
        return sizes;
    }
}

class Program
{
    private static long CalculateSquaredDistance(Point3D p1, Point3D p2)
    {
        //  d(p,q)={\sqrt {(p_{1}-q_{1})^{2}+(p_{2}-q_{2})^{2}+(p_{3}-q_{3})^{2}}}.}
        long dx = (long)p1.X - (long)p2.X;
        long dy = (long)p1.Y - (long)p2.Y;
        long dz = (long)p1.Z - (long)p2.Z;

        return (dx * dx) + (dy * dy) + (dz * dz);
    }

    static void Main(string[] args)
    {
        const string filePath = "instructions.txt";
        List<Point3D> dataPoints = new List<Point3D>();

        try
        {         
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Trim().Split(',');

                if (parts.Length == 3)
                {
                    if (int.TryParse(parts[0], out int x) &&
                        int.TryParse(parts[1], out int y) &&
                        int.TryParse(parts[2], out int z))
                    {
                        dataPoints.Add(new Point3D(x, y, z));
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Skipping unparseable line: {line}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during file processing: {ex.Message}");
            return;
        }

        int N = dataPoints.Count;
        Console.WriteLine($"Successfully parsed {N} junction boxes.");
        
        // Generate and sort all connections
        Console.WriteLine($"\nGenerating all {N * (N - 1) / 2} possible connections...");
        List<Edge> allEdges = new List<Edge>();
        for (int i = 0; i < N; i++)
        {
            for (int j = i + 1; j < N; j++)
            {
                long distance = CalculateSquaredDistance(dataPoints[i], dataPoints[j]);
                allEdges.Add(new Edge(i, j, distance));
            }
        }
        
        // Sort by the squared distance
        List<Edge> sortedEdges = allEdges
        .OrderBy(e => e.SquaredDistance)
        .ThenBy(e => e.U)
        .ThenBy(e => e.V)
        .ToList();
        

        DSU dsu = new DSU(N);

        int circuitsRemaining = N;
        Edge? lastConnectingEdge = null;

        foreach (Edge edge in sortedEdges)
        {
            if (circuitsRemaining == 1)
                break;

            if (dsu.Union(edge.U, edge.V))
            {
                circuitsRemaining--;
                lastConnectingEdge = edge;
            }
        }

        int uIndex = lastConnectingEdge.U;
        int vIndex = lastConnectingEdge.V;

        double x1 = dataPoints[uIndex].X;
        double x2 = dataPoints[vIndex].X;

        long finalProduct = (long)x1 * (long)x2;

        Console.WriteLine($"Product of the X coordinates of the last two connected boxes: {finalProduct}");
    }
}