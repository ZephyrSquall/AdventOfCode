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
        return Solve(strippedPuzzleInput);
    }

    public override long SolvePart2()
    {
        string puzzleInput = File.ReadAllText(PuzzleInputPath);
        // Strip all alphabetical characters and colons. For part 2, also strip all spaces; this is the only difference.
        string strippedPuzzleInput = new string(puzzleInput.Where(c => !(Char.IsLetter(c) || c == ':' || c == ' ')).ToArray());
        return Solve(strippedPuzzleInput);
    }


    long Solve(string strippedPuzzleInput)
    {
        string[] lines = strippedPuzzleInput.Split('\n');
        string timeLine = lines[0];
        string distanceLine = lines[1];

        long[] times = GetNumbersFromString(timeLine);
        long[] distances = GetNumbersFromString(distanceLine);

        long recordBeatenProduct = 1;

        for (long i = 0; i < times.Length; i++)
        {
            long timeLimit = times[i];
            long distanceRecord = distances[i];

            long timesRecordBeaten = GetDistanceTravelled(timeLimit, distanceRecord);
            recordBeatenProduct *= timesRecordBeaten;
        }

        return recordBeatenProduct;
    }

    long GetDistanceTravelled(long timeLimit, long distanceRecord)
    {
        // distanceTravelled as a function of timeHeldButton is a quadratic:
        //
        // speed = timeHeldButton
        // travelTime = timeLimit - timeHeldButton
        // distanceTravelled = speed * travelTime
        // distanceTravelled = (timeHeldButton) * (timeLimit - timeHeldButton)
        // distanceTravelled = timeHeldButton * timeLimit - timeHeldButton ^ 2
        //
        //
        // Want to find where distanceTravelled > distanceRecord:
        //
        // distanceTravelled > distanceRecord
        // timeHeldButton * timeLimit - timeHeldButton ^ 2 > distanceRecord
        // 0 > timeHeldButton ^ 2 - timeHeldButton * timeLimit + distanceRecord
        //
        //
        // By the quadratic formula:
        //
        // timeHeldButton = (timeLimit + sqrt(timeLimit ^ 2 - 4 * distanceRecord))/2 and (timeLimit - sqrt(timeLimit ^ 2 - 4 * distanceRecord))/2

        double timeButtonHeldFirstRoot = (timeLimit - Math.Sqrt(Math.Pow(timeLimit, 2) - 4 * distanceRecord)) / 2;
        double timeButtonHeldSecondRoot = (timeLimit + Math.Sqrt(Math.Pow(timeLimit, 2) - 4 * distanceRecord)) / 2;

        // The above variables give the exact value where the time spent holding the button will cause us to tie the record for the first and last time.
        // We want to know when we beat the record, which happens every integer starting from the integer after the first root until at the integer before the last root.
        long timeButtonHeldFirstWin = (long)Math.Ceiling(timeButtonHeldFirstRoot);
        // Check if root itself is an integer (with some tolerance for floating point precision). If so, add 1 to timeButtonHeldFirstWin if so to avoid counting ties with the distance record.
        if (Math.Abs(timeButtonHeldFirstRoot % 1) <= (Double.Epsilon * 100))
        {
            timeButtonHeldFirstWin++;
        }
        // Likewise, if the second root is an integer, subtract 1 from timeButtonHeldLastWin to avoid counting ties.
        long timeButtonHeldLastWin = (long)Math.Floor(timeButtonHeldSecondRoot);
        if (Math.Abs(timeButtonHeldSecondRoot % 1) <= (Double.Epsilon * 100))
        {
            timeButtonHeldLastWin--;
        }

        long timesRecordBeaten = timeButtonHeldLastWin - timeButtonHeldFirstWin + 1;
        return timesRecordBeaten;
    }

    long[] GetNumbersFromString(string input)
    {
        string[] splitInput = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        long[] numbers = splitInput.Select(x => long.Parse(x)).ToArray();
        return numbers;
    }
}
