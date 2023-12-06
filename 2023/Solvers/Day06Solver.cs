namespace AdventOfCode2023;

class Day06Solver : Solver
{
    private string _puzzleInputPath = "PuzzleInputs/06.txt";
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override long SolvePart1()
    {
        string puzzleInput = File.ReadAllText(PuzzleInputPath);
        // Strip all alphabetical characters and colons.
        string strippedPuzzleInput = new string(puzzleInput.Where(c => !(Char.IsLetter(c) || c == ':')).ToArray());
        string[] lines = strippedPuzzleInput.Split('\n');
        string timeLine = lines[0];
        string distanceLine = lines[1];

        int[] times = GetNumbersFromString(timeLine);
        int[] distances = GetNumbersFromString(distanceLine);

        int recordBeatenProduct = 1;

        for (int i = 0; i < times.Length; i++)
        {
            int timeLimit = times[i];
            int distanceRecord = distances[i];

            int timesRecordBeaten = 0;
            for (int j = 0; j < timeLimit; j++)
            {
                int distanceTravelled = GetDistanceTravelled(timeLimit, j, distanceRecord);

                if (distanceTravelled > distanceRecord)
                {
                    timesRecordBeaten++;
                }
            }

            Console.WriteLine(timesRecordBeaten);
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

            recordBeatenProduct *= timesRecordBeaten;
        }

        return recordBeatenProduct;
    }

    public override long SolvePart2()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);

        return 0;
    }


    int GetDistanceTravelled(int timeLimit, int timeHeldButton, int distanceRecord)
    {
        // speed = timeHeldButton
        // travelTime = timeLimit - timeHeldButton
        // distanceTravelled = speed * travelTime
        // distanceTravelled = (timeHeldButton) * (timeLimit - timeHeldButton)
        // distanceTravelled = timeHeldButton * timeLimit - timeHeldButton ^ 2
        //
        // Want to find where distanceTravelled > distanceRecord
        // distanceTravelled > distanceRecord
        // timeHeldButton * timeLimit - timeHeldButton ^ 2 > distanceRecord
        // 0 > timeHeldButton ^ 2 - timeHeldButton * timeLimit + distanceRecord
        //
        // By the quadratic formula:
        // timeHeldButton = (- timeLimit + sqrt(timeLimit ^ 2 - 4 * distanceRecord))/2 and (- timeLimit - sqrt(timeLimit ^ 2 - 4 * distanceRecord))/2
        double timeButtonHeldFirstRoot = (-timeLimit + Math.Sqrt(Math.Pow(timeLimit, 2) - 4 * distanceRecord)) / 2;
        double timeButtonHeldSecondRoot = (-timeLimit - Math.Sqrt(Math.Pow(timeLimit, 2) - 4 * distanceRecord)) / 2;
        Console.WriteLine(timeButtonHeldFirstRoot);
        Console.WriteLine(timeButtonHeldSecondRoot);
        Console.WriteLine();
        int speed = timeHeldButton;
        int travelTime = timeLimit - timeHeldButton;
        int distanceTravelled = speed * travelTime;
        return distanceTravelled;
    }

    int[] GetNumbersFromString(string input)
    {
        string[] splitInput = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        int[] numbers = splitInput.Select(x => int.Parse(x)).ToArray();
        return numbers;
    }
}
