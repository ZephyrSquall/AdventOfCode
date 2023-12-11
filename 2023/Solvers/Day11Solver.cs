namespace AdventOfCode2023;

class Day11Solver : Solver
{
    private string _puzzleTitle = "Cosmic Expansion";
    private string _puzzleInputPath = "PuzzleInputs/11.txt";
    public override string PuzzleTitle { get => _puzzleTitle; }
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override long SolvePart1()
    {
        return Solve(2);
    }

    public override long SolvePart2()
    {
        return Solve(1000000);
    }


    long Solve(int expansionMultiplier)
    {
        string[] space = File.ReadAllLines(PuzzleInputPath);

        int[] emptyRows = getEmptyRows(space);
        int[] emptyColumns = getEmptyColumns(space);

        (int y, int x)[] galaxyCoordinates = GetGalaxyCoordinates(space, emptyRows, emptyColumns, expansionMultiplier);

        long shortestPathSum = 0;
        // By ensuring j is always greater than i, every pair of galaxies is checked only once.
        for (int i = 0; i < galaxyCoordinates.Length; i++)
        {
            for (int j = i + 1; j < galaxyCoordinates.Length; j++)
            {
                shortestPathSum += ShortestPath(galaxyCoordinates[i], galaxyCoordinates[j]);
            }
        }

        return shortestPathSum;
    }

    int[] getEmptyRows(string[] space)
    {
        List<int> emptyRows = new List<int>();
        for (int y = 0; y < space.Length; y++)
        {
            // Check for empty rows by seeing if every character in the row is '.'
            if (space[y].All(point => point == '.')) emptyRows.Add(y);
        }

        return emptyRows.ToArray();
    }

    int[] getEmptyColumns(string[] space)
    {
        List<int> emptyColumns = new List<int>();

        for (int x = 0; x < space[0].Length; x++)
        {
            // Check for empty columns by checking if any row has a '#' in position x and rejecting the column if it does.
            bool isColumnEmpty = true;
            for (int y = 0; y < space.Length; y++)
            {
                if (space[y][x] == '#')
                {
                    isColumnEmpty = false;
                    break;
                }
            }

            if (isColumnEmpty) emptyColumns.Add(x);
        }

        return emptyColumns.ToArray();
    }

    (int y, int x)[] GetGalaxyCoordinates(string[] space, int[] emptyRows, int[] emptyColumns, int expansionMultiplier)
    {
        HashSet<(int y, int x)> galaxyCoordinates = new HashSet<(int y, int x)>();

        int expandedY = 0;

        for (int y = 0; y < space.Length; y++)
        {
            // If y is an empty row, increment expandedY by the size of how many empty rows that a single empty row has expanded to. Otherwise increment by 1 because rows with galaxies have no expansion.
            if (emptyRows.Contains(y))
            {
                expandedY += expansionMultiplier;
                continue;
            }
            expandedY++;

            int expandedX = 0;

            for (int x = 0; x < space[y].Length; x++)
            {
                // Likewise, increment expandX by how many empty columns that a single empty column expands to if the column is empty, otherwise increment by 1.
                if (emptyColumns.Contains(x))
                {
                    expandedX += expansionMultiplier;
                    continue;
                }
                expandedX++;

                if (space[y][x] == '#') galaxyCoordinates.Add((expandedY, expandedX));
            }
        }

        return galaxyCoordinates.ToArray();
    }

    int ShortestPath((int y, int x) galaxy1, (int y, int x) galaxy2)
    {
        int yDiff = Math.Abs(galaxy2.y - galaxy1.y);
        int xDiff = Math.Abs(galaxy2.x - galaxy1.x);
        return yDiff + xDiff;
    }
}
