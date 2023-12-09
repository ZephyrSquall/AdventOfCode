namespace AdventOfCode2023;

class Day09Solver : Solver
{
    private string _puzzleTitle = "Mirage Maintenance";
    private string _puzzleInputPath = "PuzzleInputs/09.txt";
    public override string PuzzleInputPath { get => _puzzleInputPath; }
    public override string PuzzleTitle { get => _puzzleTitle; }

    public override long SolvePart1()
    {
        return Solve(false);
    }

    public override long SolvePart2()
    {
        return Solve(true);
    }


    int Solve(bool goBackwards)
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);
        int nextValueSum = 0;

        foreach (string line in lines)
        {
            int[] values = GetNumbersFromString(line);

            int nextValue = GetNextValue(values, goBackwards);
            nextValueSum += nextValue;
        }

        return nextValueSum;
    }

    int GetNextValue(int[] values, bool goBackwards)
    {
        // Get new array with the differences between the values.
        int[] diffs = new int[values.Length - 1];
        for (int i = 0; i < diffs.Length; i++) diffs[i] = values[i + 1] - values[i];

        // If any value in the diffs array is not zero, recursively call this function to get the innerNextValue (the value appended to the diffs array).
        // Otherwise (meaning diffs array is all zeroes), innerNextValue is 0.
        int innerNextValue = 0;
        if (diffs.Any(diff => diff != 0)) innerNextValue = GetNextValue(diffs, goBackwards);

        // If going forwards:
        //   innerNextValue is be the difference between the last value and nextValue, i.e.
        //     innerNextValue = nextValue - values[^1]
        //   therefore:
        //     nextValue = innerNextValue + values[^1]
        //
        // If going backwards:
        //   innerNextValue is be the difference between nextValue and the first value, i.e.
        //     innerNextValue = values[0] - nextValue
        //   therefore:
        //     nextValue = values[0] - innerNextValue
        if (goBackwards) return values[0] - innerNextValue;
        else return innerNextValue + values[^1];
    }

    int[] GetNumbersFromString(string input)
    {
        string[] splitInput = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        int[] numbers = splitInput.Select(x => int.Parse(x)).ToArray();
        return numbers;
    }
}
