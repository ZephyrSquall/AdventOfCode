namespace AdventOfCode2023;

class Day01Solver : Solver
{
    private string _puzzleInputPath = "PuzzleInputs/01.txt";
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override long SolvePart1()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);

        int count = 0;

        foreach (string line in lines)
        {
            bool foundFirstDigit = false;
            int firstDigit = -1;
            int lastDigit = -1;

            foreach (char c in line)
            {
                if (Char.IsDigit(c))
                {
                    int value = (int)Char.GetNumericValue(c);
                    UpdateDigits(value, ref foundFirstDigit, ref firstDigit, ref lastDigit);
                }
            }

            int calibrationValue = 10 * firstDigit + lastDigit;
            count += calibrationValue;
        }

        return count;
    }

    public override long SolvePart2()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);

        int count = 0;

        foreach (string line in lines)
        {
            bool foundFirstDigit = false;
            int firstDigit = -1;
            int lastDigit = -1;

            char[] characters = line.ToCharArray();
            for (int i = 0; i < characters.Length; i++)
            {
                char character = characters[i];

                if (Char.IsDigit(character))
                {
                    int value = (int)Char.GetNumericValue(character);
                    UpdateDigits(value, ref foundFirstDigit, ref firstDigit, ref lastDigit);
                }

                if (Char.IsLetter(character))
                {
                    int? nullableValue = CheckWrittenNumber(characters, i);
                    if (nullableValue is int value)
                    {
                        UpdateDigits(value, ref foundFirstDigit, ref firstDigit, ref lastDigit);
                    }
                }
            }

            int calibrationValue = 10 * firstDigit + lastDigit;
            count += calibrationValue;
        }

        return count;
    }


    void UpdateDigits(int value, ref bool foundFirstDigit, ref int firstDigit, ref int lastDigit)
    {
        lastDigit = value;
        if (!foundFirstDigit)
        {
            firstDigit = value;
            foundFirstDigit = true;
        }
    }

    int? CheckWrittenNumber(char[] characters, int position)
    {
        int charactersRemaining = characters.Length - position;

        if (charactersRemaining >= 3)
        {
            if (characters[position] == 'o' && characters[position + 1] == 'n' && characters[position + 2] == 'e') return 1;
            if (characters[position] == 't' && characters[position + 1] == 'w' && characters[position + 2] == 'o') return 2;
            if (characters[position] == 's' && characters[position + 1] == 'i' && characters[position + 2] == 'x') return 6;
        }
        if (charactersRemaining >= 4)
        {
            if (characters[position] == 'f' && characters[position + 1] == 'o' && characters[position + 2] == 'u' && characters[position + 3] == 'r') return 4;
            if (characters[position] == 'f' && characters[position + 1] == 'i' && characters[position + 2] == 'v' && characters[position + 3] == 'e') return 5;
            if (characters[position] == 'n' && characters[position + 1] == 'i' && characters[position + 2] == 'n' && characters[position + 3] == 'e') return 9;
        }
        if (charactersRemaining >= 5)
        {
            if (characters[position] == 't' && characters[position + 1] == 'h' && characters[position + 2] == 'r' && characters[position + 3] == 'e' && characters[position + 4] == 'e') return 3;
            if (characters[position] == 's' && characters[position + 1] == 'e' && characters[position + 2] == 'v' && characters[position + 3] == 'e' && characters[position + 4] == 'n') return 7;
            if (characters[position] == 'e' && characters[position + 1] == 'i' && characters[position + 2] == 'g' && characters[position + 3] == 'h' && characters[position + 4] == 't') return 8;
        }
        return null;
    }
}
