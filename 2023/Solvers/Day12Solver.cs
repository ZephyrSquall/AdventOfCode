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

        foreach (string line in lines)
        {
            string[] splitLine = line.Split(' ');
            string springConditionRecord = splitLine[0];
            string contiguousGroupsLine = splitLine[1];
            int[] contiguousGroups = GetNumbersFromString(contiguousGroupsLine);

            validConfigurationSum += GetNumberOfValidConfigurations(springConditionRecord, contiguousGroups, 0, 0);
        }

        return validConfigurationSum;
    }

    public override long SolvePart2()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);

        return 0;
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
