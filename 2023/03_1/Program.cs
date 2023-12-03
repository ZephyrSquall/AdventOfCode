namespace _03_1;

class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines(args[0]);
        int count = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            for (int j = 0; j < line.Length; j++)
            {
                char character = line[j];
                if (Char.IsDigit(character))
                {
                    int length = GetNumberLength(line, j);
                    string numberString = line.Substring(j, length);
                    int number = int.Parse(numberString);

                    if (isAdjacentToSymbol(lines, i, j, length))
                    {
                        count += number;
                    }

                    // Skip to the end of the number to avoid evaluating other digits in the same number
                    j += length;
                }
            }
        }

        Console.WriteLine(count);
    }

    static int GetNumberLength(string line, int startingPosition)
    {
        int endingPosition = startingPosition + 1;

        while (endingPosition < line.Length && Char.IsDigit(line[endingPosition]))
        {
            endingPosition++;
        }

        return endingPosition - startingPosition;
    }
    static bool isAdjacentToSymbol(string[] lines, int startingLine, int startingPosition, int length)
    {
        for (int i = startingLine - 1; i <= startingLine + 1; i++)
        {
            if (i >= 0 && i < lines.Length)
            {
                // Do not add 1 to "j <= startingPosition + length" condition since 1 also needs to be subtracted to account for a length of 1 not widening the search box, which cancels out.
                for (int j = startingPosition - 1; j <= startingPosition + length; j++)
                {
                    if (j >= 0 && j < lines[i].Length)
                    {
                        char character = lines[i][j];
                        if (character != '.' && !Char.IsDigit(character))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}
