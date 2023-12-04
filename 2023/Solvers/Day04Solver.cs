namespace AdventOfCode2023;

class Day04Solver : Solver
{
    private string _puzzleInputPath = "PuzzleInputs/04.txt";
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override int SolvePart1()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);
        int count = 0;

        foreach (string line in lines)
        {
            // Trim everything before colon, as card number is irrelevant. Uses range operator.
            string trimmedLine = line[(line.IndexOf(':') + 1)..];

            string[] numberGroups = trimmedLine.Split('|');

            int[] winningNumbers = GetNumbersFromString(numberGroups[0]);
            int[] myNumbers = GetNumbersFromString(numberGroups[1]);

            int matches = 0;
            foreach (int myNumber in myNumbers)
            {
                foreach (int winningNumber in winningNumbers)
                {
                    if (myNumber == winningNumber)
                    {
                        matches++;
                        continue;
                    }
                }
            }

            int points = GetPoints(matches);
            count += points;
        }

        return count;
    }

    public override int SolvePart2()
    {
        return 0;
    }


    int[] GetNumbersFromString(string input)
    {
        string[] splitInput = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        int[] numbers = splitInput.Select(x => int.Parse(x)).ToArray();
        return numbers;
    }

    int GetPoints(int matches)
    {
        if (matches == 0)
        {
            return 0;
        }
        else
        {
            int result = 1;
            // Start loop index from 1 instead of 0 because first step is setting result to 1, already done above. 
            for (int i = 1; i < matches; i++)
            {
                result *= 2;
            }
            return result;
        }
    }
}
