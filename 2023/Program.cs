﻿namespace AdventOfCode2023;

class Program
{
    static void Main(string[] args)
    {
        Dictionary<int, Solver> solvers = new Dictionary<int, Solver>
        {
            [1] = new Day01Solver(),
            [2] = new Day02Solver(),
            [3] = new Day03Solver(),
            [4] = new Day04Solver(),
            [5] = new Day05Solver(),
            [6] = new Day06Solver(),
            [7] = new Day07Solver(),
        };

        if (args.Length == 0)
        {
            Console.WriteLine("Input day to solve.");
        }
        else
        {
            int day = int.Parse(args[0]);
            long solution1 = solvers[day].SolvePart1();
            long solution2 = solvers[day].SolvePart2();
            Console.WriteLine("Part 1: {0}", solution1);
            Console.WriteLine("Part 2: {0}", solution2);
        }
    }
}
