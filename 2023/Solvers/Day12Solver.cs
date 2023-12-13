using System.Numerics;

namespace AdventOfCode2023;

class Day12Solver : Solver
{
    private string _puzzleTitle = "Hot Springs";
    private string _puzzleInputPath = "PuzzleInputs/12.txt";
    public override string PuzzleTitle { get => _puzzleTitle; }
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override long SolvePart1()
    {
        int validConfigurationSum = 0;
        string[] lines = File.ReadAllLines(PuzzleInputPath);

        //foreach (string line in lines)
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            string[] splitLine = line.Split(' ');
            string springConditionRecord = splitLine[0];
            string contiguousGroupsLine = splitLine[1];
            int[] contiguousGroups = GetNumbersFromString(contiguousGroupsLine);

            int validConfigurationNum = GetNumberOfValidConfigurations(springConditionRecord, contiguousGroups, 0, 0);
            validConfigurationSum += validConfigurationNum;

            BigInteger nonogramConfigurationNum = GetNumberOfNonogramConfigurations(springConditionRecord, contiguousGroups);

            Console.WriteLine($"{i} valid configs: {validConfigurationNum}, nonogram configs: {nonogramConfigurationNum}, difference: {nonogramConfigurationNum - validConfigurationNum} ({line})");
        }

        return validConfigurationSum;
    }

    public override long SolvePart2()
    {
        int validConfigurationSum = 0;
        string[] lines = File.ReadAllLines(PuzzleInputPath);

        // The index i is only used for logging debugging information. The foreach loop can be reinstated once logging is removed.
        //foreach (string line in lines)
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            string[] splitLine = line.Split(' ');
            string springConditionRecord = splitLine[0];
            string contiguousGroupsLine = splitLine[1];
            int[] contiguousGroups = GetNumbersFromString(contiguousGroupsLine);

            Unfold(ref springConditionRecord, ref contiguousGroups);

            // Comment out this line for the sake of letting the code complete, as the number of configurations to check is too high them all individually in a reasonable time.
            // It is unlikely that any method of exhaustively checking every combination will work without taking at least many hours to run, violating the principle that each puzzle has a solution that runs under 15 seconds.
            // int validConfigurationNum =  GetNumberOfValidConfigurations(springConditionRecord, contiguousGroups, 0, 0);

            // Set to 0 as a placeholder for now.
            int validConfigurationNum = 0;
            validConfigurationSum += validConfigurationNum;

            // The number of nonogram configurations is found using combinatorics, so it still runs efficiently on these much-larger nonograms.
            BigInteger nonogramConfigurationNum = GetNumberOfNonogramConfigurations(springConditionRecord, contiguousGroups);

            // For logging only, update line to reflect being unfolded. Can delete this when logging is removed.
            line = springConditionRecord + ", [" + contiguousGroups[0];
            for (int j = 1; j < contiguousGroups.Length; j++)
            {
                line = String.Concat(line, ", ", contiguousGroups[j]);
            }
            line = line + "]";
            Console.WriteLine($"{i}: valid configs: {validConfigurationNum}, nonogram configs: {nonogramConfigurationNum}, difference: {nonogramConfigurationNum - validConfigurationNum} ({line})");
        }

        return validConfigurationSum;
    }

    // "nonogram configurations" treats the string as if it entirely consists of '?' and finds the number of solutions, essentially turning the problem into solving a row of a nonogram puzzle.
    BigInteger GetNumberOfNonogramConfigurations(string springConditionRecord, int[] contiguousGroups)
    {
        // Get the number of slots that need to be filled with '#' or '.'
        int slots = springConditionRecord.Length;
        // Get the number of slots whose relative order is already predetermined according to the contiguous groups.
        // This is every '#' (contiguousGroups.Sum()), plus a single '.' between each contiguous group to make sure each group is separated (contiguousGroups.Length - 1).
        int filledSlots = contiguousGroups.Sum() + contiguousGroups.Length - 1;
        // Get the remaining slots, whose relative order is not predetermined.
        // These remaining slots will contain all '.', as every '#' has a relative order.
        int slotsToFill = slots - filledSlots;
        // Get the number of distinct positions the remaining '.' can be allocated to.
        // These positions are before the first contiguous group (1), between each contiguous group ((contiguousGroups.Length - 1)), and after the last contiguous group (1), for total of (contiguousGroups.Length + 1).
        int distinctPositions = contiguousGroups.Length + 1;

        // The total number of nonogram solutions is now equal to the number of distinct multisets of cardinality slotsToFill with elements taken from a set of cardinality distinctPositions
        // (We want to know how many ways we can assign "slotsToFill" elements to "distinctPositions" bins)
        // This can be computed with Choose(n + k - 1, k), where k is the cardinality of the multiset to fill and n is the cardinality of the set to take elements from.
        // See https://en.wikipedia.org/wiki/Combination#Number_of_combinations_with_repetition
        return Choose(distinctPositions + slotsToFill - 1, slotsToFill);
    }

    BigInteger Factorial(int x)
    {
        if (x == 0) return 1;
        else return x * Factorial(x - 1);
    }

    BigInteger Choose(int n, int k)
    {
        return Factorial(n) / (Factorial(k) * Factorial(n - k));
    }

    void Unfold(ref string springConditionRecord, ref int[] contiguousGroups)
    {
        springConditionRecord = springConditionRecord + '?' + springConditionRecord + '?' + springConditionRecord + '?' + springConditionRecord + '?' + springConditionRecord;
        List<int> unfoldingContiguousGroups = new List<int>(contiguousGroups);
        unfoldingContiguousGroups.AddRange(contiguousGroups);
        unfoldingContiguousGroups.AddRange(contiguousGroups);
        unfoldingContiguousGroups.AddRange(contiguousGroups);
        unfoldingContiguousGroups.AddRange(contiguousGroups);
        contiguousGroups = unfoldingContiguousGroups.ToArray();
    }

    // Form a tree out of each choice between damaged springs '#' and undamaged springs '.' at each unknown spring '?'.
    // Recursively check each branch, rejecting a branch as soon as it fails a check for validity.
    //This prevents a wide number of similar branches from being checked individually when they share similar characteristics that can get them all rejected at once.
    int GetNumberOfValidConfigurations(string springConditionRecord, int[] contiguousGroups, int springIndex, int contiguousGroupIndex)
    {
        int currentDamagedSprings = springConditionRecord.Count(spring => spring == '#');
        int maximumDamagedSprings = contiguousGroups.Sum();
        int currentUnknownSprings = springConditionRecord.Count(spring => spring == '?');

        //Console.WriteLine($"Checking {springConditionRecord}");

        // Check if the number of damaged springs exceed the total in the contiguous groups. If so, reject this branch.
        if (currentDamagedSprings > maximumDamagedSprings) return 0;
        // Check if enough unknown springs remain to reach the total in the contiguous groups. If so, reject this branch.
        if (currentUnknownSprings < maximumDamagedSprings - currentDamagedSprings) return 0;

        // If the current damaged springs equals the total in the contiguous groups, and the contiguousGroupIndex has reached the end of the contiguousGroup array to confirm that each group of damaged springs is separate, then this is a valid configuration.
        if (currentDamagedSprings == maximumDamagedSprings && contiguousGroupIndex == contiguousGroups.Length) return 1;

        while (springIndex < springConditionRecord.Length)
        {
            char spring = springConditionRecord[springIndex];
            if (spring == '#')
            {
                // Try to move to the end of the contiguous block. If unsuccessful, this damaged spring cannot be successfully grouped up according to the contiguous groups, so reject this branch.
                if (TryFillContiguousBlock(ref springConditionRecord, contiguousGroups, springIndex, contiguousGroupIndex))
                {
                    // Update indexes to account for skipping to the end of a contiguous group.
                    springIndex += contiguousGroups[contiguousGroupIndex] + 1;
                    contiguousGroupIndex++;
                    return GetNumberOfValidConfigurations(springConditionRecord, contiguousGroups, springIndex, contiguousGroupIndex);
                }
                else
                {
                    return 0;
                }
            }

            if (spring == '?')
            {
                // Split into two branches, one for damaged spring '#' and one for undamaged spring '.'.
                // For the damaged branch, try to move to the end of the contiguous block that it starts. If unsuccessful, this damaged spring cannot be successfully grouped up according to the contiguous groups, so reject the damaged branch and only proceed with the undamaged branch.
                string damagedBranchConfiguration = springConditionRecord;
                string undamagedBranchConfiguration = springConditionRecord.Remove(springIndex, 1).Insert(springIndex, ".");
                if (TryFillContiguousBlock(ref damagedBranchConfiguration, contiguousGroups, springIndex, contiguousGroupIndex))
                {
                    // Update indexes to account for skipping to the end of a contiguous group. Make sure the updated indexes are used only for the damaged spring branch, not the undamaged spring branch.
                    int damageSpringIndex = springIndex + contiguousGroups[contiguousGroupIndex];
                    return GetNumberOfValidConfigurations(undamagedBranchConfiguration, contiguousGroups, springIndex + 1, contiguousGroupIndex) + GetNumberOfValidConfigurations(damagedBranchConfiguration, contiguousGroups, damageSpringIndex, contiguousGroupIndex + 1);
                }
                else
                {
                    // Proceeding only with the undamaged branch.
                    return GetNumberOfValidConfigurations(undamagedBranchConfiguration, contiguousGroups, springIndex + 1, contiguousGroupIndex);
                }
            }

            // If code reaches this part of the loop, the spring is an undamaged spring '.', which requires no special handling, so move to the next index and immediately check the spring again.
            springIndex++;
        }

        return 0;
    }


    bool TryFillContiguousBlock(ref string springConditionRecord, int[] contiguousGroups, int springIndex, int contiguousGroupIndex)
    {
        string tempSpringConditionRecord = springConditionRecord;

        // For the length of the contiguous group, replace all unknowns '?' with damaged springs '#' as it's the only way to get a valid configuration.
        // If an undamaged spring '.' is encountered, return false to indicate that a contiguous group cannot fit here.
        for (int i = springIndex; i - springIndex < contiguousGroups[contiguousGroupIndex]; i++)
        {
            if (i >= springConditionRecord.Length) return false;

            char spring = tempSpringConditionRecord[i];
            if (spring == '.') return false;
            else if (spring == '?') tempSpringConditionRecord = tempSpringConditionRecord.Remove(i, 1).Insert(i, "#");
        }

        // All contiguous groups much be separated by at least one undamaged spring. So if the spring after the contiguous group is an unknown, replace it with an undamaged spring.
        // If the spring after the contiguous group is an undamaged spring, return false to indicate that this contiguous group would merge with another if it were placed here.
        int gapIndex = springIndex + contiguousGroups[contiguousGroupIndex];
        // First check if the index is past the end of the string, as an undamaged spring isn't required if there are no more springs. This check is required to avoid an ArgumentOutOfRangeException.
        if (gapIndex < springConditionRecord.Length)
        {
            char spring = tempSpringConditionRecord[gapIndex];
            if (spring == '#') return false;
            else if (spring == '?') tempSpringConditionRecord = tempSpringConditionRecord.Remove(gapIndex, 1).Insert(gapIndex, ".");
        }

        springConditionRecord = tempSpringConditionRecord;
        return true;
    }


    int[] GetNumbersFromString(string input)
    {
        string[] splitInput = input.Split(',');
        int[] numbers = splitInput.Select(x => int.Parse(x)).ToArray();
        return numbers;
    }
}
