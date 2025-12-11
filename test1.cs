using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Google.OrTools.LinearSolver;


partial class Program
{
    static void Main()
    {
       string inputFile = "instructions.txt";

       string[] lines = File.ReadAllLines(inputFile);

       Dictionary<string, List<string>> graph = [];

       foreach (var line in lines)
        {
            if(string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(":");
            var node = parts[0].Trim();
            var targets = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if(!graph.ContainsKey(node))
            {
                graph[node] = [];
            }

            foreach(var target in targets)
            {
                graph[node].Add(target.Trim());
            }
        }

        int result = CountPaths(graph, "you", "out", new HashSet<string>());
        Console.WriteLine($"Number of distinct paths from 'you' to 'out': {result}");

    }

    static int CountPaths(
        Dictionary<string, List<string>> graph,
        string current,
        string target,
        HashSet<string> visited)
    {

        if (current == target)
        {
            return 1;
        }

        visited.Add(current);
        int pathCount = 0;

        if (graph.ContainsKey(current))
        {
            foreach (var next in graph[current])
            {
                if (!visited.Contains(next))
                {
                    pathCount += CountPaths(graph, next, target, [.. visited]);
                }
            }
        }

        return pathCount;
    }

}
