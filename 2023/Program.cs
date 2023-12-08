using System.Diagnostics;

namespace AdventOfCode2023;

class Program
{
    const string DAY_TITLE = "Day";
    const string PUZZLE_TITLE = "Puzzle";
    const string PART_TITLE = "Part";
    const string SOLUTION_TITLE = "Solution";
    const string TIMING_TITLE = "Time (ms)";
    static readonly Dictionary<int, Solver> solvers = new Dictionary<int, Solver>
    {
        [1] = new Day01Solver(),
        [2] = new Day02Solver(),
        [3] = new Day03Solver(),
        [4] = new Day04Solver(),
        [5] = new Day05Solver(),
        [6] = new Day06Solver(),
        [7] = new Day07Solver(),
        [8] = new Day08Solver(),
    };

    static void Main(string[] args)
    {
        List<DayResult> dayResults = new List<DayResult>();

        if (args.Length == 0)
        {
            foreach (KeyValuePair<int, Solver> keyValuePair in solvers)
            {
                dayResults.Add(new DayResult(keyValuePair.Key));
            }
        }
        else
        {
            foreach (string arg in args)
            {
                int day = int.Parse(arg);
                dayResults.Add(new DayResult(day));
            }
        }

        MaxLengths maxLengths = new MaxLengths
        {
            maxDayLength = dayResults.Aggregate(DAY_TITLE.Length, (currentMaximum, dayResult) => Math.Max(currentMaximum, dayResult.day.Length)),
            maxPuzzleLength = dayResults.Aggregate(PUZZLE_TITLE.Length, (currentMaximum, dayResult) => Math.Max(currentMaximum, dayResult.puzzleTitle.Length)),
            maxPartLength = PART_TITLE.Length,
            maxSolutionLength = dayResults.Aggregate(SOLUTION_TITLE.Length, (currentMaximum, dayResult) => Math.Max(currentMaximum, Math.Max(dayResult.solution1.Length, dayResult.solution2.Length))),
            maxTimingLength = dayResults.Aggregate(TIMING_TITLE.Length, (currentMaximum, dayResult) => Math.Max(currentMaximum, Math.Max(dayResult.time1.Length, dayResult.time2.Length))),
        };

        WriteHeader(maxLengths);
        bool firstRow = true;
        for (int i = 0; i < dayResults.Count; i++)
        {
            if (firstRow)
            {
                firstRow = false;
            }
            else
            {
                WriteEmptyRow(maxLengths);
            }
            WriteRow(dayResults[i], maxLengths);
        }
        WriteFinalRow(maxLengths);
    }


    class DayResult
    {
        public string day;
        public string puzzleTitle;
        public string solution1;
        public string solution2;
        public string time1;
        public string time2;

        public DayResult(int day)
        {
            this.day = day.ToString();
            puzzleTitle = solvers[day].PuzzleTitle;

            long solution1;
            Stopwatch stopwatch = Stopwatch.StartNew();
            solution1 = solvers[day].SolvePart1();
            stopwatch.Stop();

            double time1 = stopwatch.Elapsed.TotalMilliseconds;
            this.solution1 = solution1.ToString();
            this.time1 = time1.ToString();

            long solution2;
            stopwatch.Reset();
            stopwatch.Start();
            solution2 = solvers[day].SolvePart2();
            stopwatch.Stop();

            double time2 = stopwatch.Elapsed.TotalMilliseconds;
            this.solution2 = solution2.ToString();
            this.time2 = time2.ToString();
        }
    }

    class MaxLengths
    {
        public int maxDayLength;
        public int maxPuzzleLength;
        public int maxPartLength;
        public int maxSolutionLength;
        public int maxTimingLength;
    }

    static void WriteHeader(MaxLengths maxLengths)
    {
        Console.WriteLine($"╔═{new string('═', maxLengths.maxDayLength)}═╤═{new string('═', maxLengths.maxPuzzleLength)}═╤═{new string('═', maxLengths.maxPartLength)}═╤═{new string('═', maxLengths.maxSolutionLength)}═╤═{new string('═', maxLengths.maxTimingLength)}═╗");
        Console.WriteLine($"║ {DAY_TITLE.PadRight(maxLengths.maxDayLength)} │ {PUZZLE_TITLE.PadRight(maxLengths.maxPuzzleLength)} │ {PART_TITLE.PadRight(maxLengths.maxPartLength)} │ {SOLUTION_TITLE.PadRight(maxLengths.maxSolutionLength)} │ {TIMING_TITLE.PadRight(maxLengths.maxTimingLength)} ║");
        Console.WriteLine($"╟─{new string('─', maxLengths.maxDayLength)}─┼─{new string('─', maxLengths.maxPuzzleLength)}─┼─{new string('─', maxLengths.maxPartLength)}─┼─{new string('─', maxLengths.maxSolutionLength)}─┼─{new string('─', maxLengths.maxTimingLength)}─╢");
    }

    static void WriteRow(DayResult dayResult, MaxLengths maxLengths)
    {
        Console.WriteLine($"║ {dayResult.day.PadLeft(maxLengths.maxDayLength)} │ {dayResult.puzzleTitle.PadRight(maxLengths.maxPuzzleLength)} │ {"1".PadLeft(maxLengths.maxPartLength)} │ {dayResult.solution1.PadLeft(maxLengths.maxSolutionLength)} │ {dayResult.time1.PadLeft(maxLengths.maxTimingLength)} ║");
        Console.WriteLine($"║ {new string(' ', maxLengths.maxDayLength)} │ {new string(' ', maxLengths.maxPuzzleLength)} │ {"2".PadLeft(maxLengths.maxPartLength)} │ {dayResult.solution2.PadLeft(maxLengths.maxSolutionLength)} │ {dayResult.time2.PadLeft(maxLengths.maxTimingLength)} ║");
    }

    static void WriteEmptyRow(MaxLengths maxLengths)
    {
        Console.WriteLine($"║ {new string(' ', maxLengths.maxDayLength)} │ {new string(' ', maxLengths.maxPuzzleLength)} │ {new string(' ', maxLengths.maxPartLength)} │ {new string(' ', maxLengths.maxSolutionLength)} │ {new string(' ', maxLengths.maxTimingLength)} ║");
    }

    static void WriteFinalRow(MaxLengths maxLengths)
    {
        Console.WriteLine($"╚═{new string('═', maxLengths.maxDayLength)}═╧═{new string('═', maxLengths.maxPuzzleLength)}═╧═{new string('═', maxLengths.maxPartLength)}═╧═{new string('═', maxLengths.maxSolutionLength)}═╧═{new string('═', maxLengths.maxTimingLength)}═╝");
    }
}
