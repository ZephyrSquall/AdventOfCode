namespace _03_2;

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
                if (character == '*')
                {
                    int[] numbers = getAdjacentNumbers(lines, i, j);

                    if (numbers.Length == 2)
                    {
                        int gearRatio = numbers[0] * numbers[1];
                        count += gearRatio;
                    }
                }
            }
        }

        Console.WriteLine(count);
    }

    static int[] getAdjacentNumbers(string[] lines, int startingLine, int startingPosition)
    {
        List<int> numbers = new List<int>();

        for (int i = startingLine - 1; i <= startingLine + 1; i++)
        {
            if (i >= 0 && i < lines.Length)
            {
                // Of the three adjacent characters on row above and the row below the gear, if the middle character is a digit, then all the digits in that row must be connected and there is exactly one adjacent number in that row. Otherwise, the two outside characters in that row are disconnected so any digits in those positions belong to separate adjacent numbers.
                // Hence process is to check middle character in a row first, and only if it is not a digit do the outer two characters in that row get checked next. This also technically applies to the middle row, though the center character is the asterisk representing the gear so it is never a digit.
                bool middleCharacterIsDigit = Char.IsDigit(lines[i][startingPosition]);
                if (middleCharacterIsDigit)
                {
                    int number = GetNumberValue(lines[i], startingPosition);
                    numbers.Add(number);
                }
                else
                {
                    int[] remainingPositions = [startingPosition - 1, startingPosition + 1];
                    foreach (int j in remainingPositions)
                    {
                        if (j >= 0 && j < lines[i].Length)
                        {
                            char character = lines[i][j];
                            if (Char.IsDigit(character))
                            {
                                int number = GetNumberValue(lines[i], j);
                                numbers.Add(number);
                            }
                        }
                    }
                }
            }
        }

        return numbers.ToArray();
    }

    static int GetNumberValue(string line, int linePosition)
    {
        int startingPosition = linePosition;
        int endingPosition = linePosition;

        while (startingPosition > 0 && Char.IsDigit(line[startingPosition - 1]))
        {
            startingPosition--;
        }

        while (endingPosition < line.Length - 1 && Char.IsDigit(line[endingPosition + 1]))
        {
            endingPosition++;
        }

        int length = endingPosition - startingPosition + 1;
        string numberString = line.Substring(startingPosition, length);
        int number = int.Parse(numberString);

        return number;
    }
}
