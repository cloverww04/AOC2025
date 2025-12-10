using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Google.OrTools.LinearSolver;

public class Manual
{
    public List<List<int>> Buttons { get; set; } = [];
    public List<int> Joltage { get; set; } = [];
}

partial class Program
{
    static void Main()
    {
        var manuals = ParseManuals("instructions.txt");

        long total = 0;
        foreach (var manual in manuals)
        {
            int presses = SolveMachineJoltage_ILP(manual);
            total += presses;
        }

        Console.WriteLine($"\nTotal presses required = {total}");
    }

    static List<Manual> ParseManuals(string path)
    {
        var lines = File.ReadAllLines(path);
        var manuals = new List<Manual>();

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmed))
                continue;

            var buttonMatches = MyRegex().Matches(trimmed);
            var buttons = new List<List<int>>();
            foreach (Match m in buttonMatches)
            {
                var nums = m.Groups[1].Value
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToList();
                buttons.Add(nums);
            }

            var jMatch = MyRegex1().Match(trimmed);

            var joltage = jMatch.Groups[1].Value
                           .Split(',', StringSplitOptions.RemoveEmptyEntries)
                           .Select(int.Parse)
                           .ToList();

            manuals.Add(new Manual
            {
                Buttons = buttons,
                Joltage = joltage
            });
        }

        return manuals;
    }

    static int SolveMachineJoltage_ILP(Manual manual)
    {
        int numButtons = manual.Buttons.Count;
        int numCounters = manual.Joltage.Count;

        Solver solver = Solver.CreateSolver("SCIP") ?? throw new Exception("Could not create solver.");
        Variable[] x = new Variable[numButtons];
        for (int i = 0; i < numButtons; i++)
            x[i] = solver.MakeIntVar(0.0, double.PositiveInfinity, $"x{i}");

        for (int j = 0; j < numCounters; j++)
        {
            LinearExpr? sum = x.Length > 0 ? x[0] * 0 : null;
            for (int i = 0; i < numButtons; i++)
            {
                if (manual.Buttons[i].Contains(j))
                    sum += x[i];
            }
            solver.Add(sum == manual.Joltage[j]);
        }

        LinearExpr? objective = x.Length > 0 ? x[0] * 0 : null;
        foreach (var xi in x)
            objective += xi;

        solver.Minimize(objective);

        var resultStatus = solver.Solve();
        if (resultStatus != Solver.ResultStatus.OPTIMAL)
            throw new Exception("No optimal solution found for a machine.");

        return (int)solver.Objective().Value();
    }

    [GeneratedRegex(@"\((.*?)\)")]
    private static partial Regex MyRegex();
    [GeneratedRegex(@"\{(.*?)\}")]
    private static partial Regex MyRegex1();
}
