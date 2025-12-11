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

        long result = CountPaths(graph, "svr", "out", new HashSet<string>(), false, false);
        Console.WriteLine($"Number of distinct paths from 'svr' to 'out': {result}");

    }

    // sticky note: memoization to avoid recomputing paths
    static Dictionary<(string, bool, bool), long> memo = new();

    static long CountPaths(
        Dictionary<string, List<string>> graph,
        string current,
        string target,
        HashSet<string> visited,
        bool seenDac,
        bool seenFft)
    {
        if (current == "dac") seenDac = true;
        if (current == "fft") seenFft = true;

        // look at the sticky note to avoid counting from scratch every time
        var key = (current, seenDac, seenFft);
        if (memo.TryGetValue(key, out long cached))
            return cached;

        if (current == target)
        {
            long result = (seenDac && seenFft) ? 1 : 0;
            memo[key] = result;
            return result;
        }

        if (visited.Contains(current))
            return 0;

        visited.Add(current);
        long total = 0;

        if (graph.ContainsKey(current))
        {
            foreach (var next in graph[current])
            {
                total += CountPaths(
                    graph,
                    next,
                    target,
                    visited,
                    seenDac,
                    seenFft
                );
            }
        }

        visited.Remove(current);
        memo[key] = total;
        return total;
    }

}
