namespace AdventOfCode2023;

class Day11Solver : Solver
{
    private string _puzzleTitle = "Cosmic Expansion";
    private string _puzzleInputPath = "PuzzleInputs/11.txt";
    public override string PuzzleTitle { get => _puzzleTitle; }
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override long SolvePart1()
    {
        string[] space = File.ReadAllLines(PuzzleInputPath);

        space = ExpandSpace(space);

        (int y, int x)[] galaxyCoordinates = GetGalaxyCoordinates(space);

        int shortestPathSum = 0;
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

    public override long SolvePart2()
    {
        string[] space = File.ReadAllLines(PuzzleInputPath);
        return 0;
    }


    string[] ExpandSpace(string[] space)
    {
        List<int> rowsToExpand = new List<int>();
        List<int> columnsToExpand = new List<int>();

        for (int y = 0; y < space.Length; y++)
        {
            // Check for empty rows by seeing if every character in the row is '.'
            if (space[y].All(point => point == '.')) rowsToExpand.Add(y);
        }

        for (int x = 0; x < space[0].Length; x++)
        {
            // Check for empty columns by checking if any row has a '#' in position x and rejecting the column if it does.
            bool emptyColumn = true;
            for (int y = 0; y < space.Length; y++)
            {
                if (space[y][x] == '#')
                {
                    emptyColumn = false;
                    break;
                }
            }

            if (emptyColumn) columnsToExpand.Add(x);
        }

        // Expand space by iterating the list in reverse order (i.e. starting with the last row/column), since adding an extra row/column changes the index of all following rows/columns.
        List<string> spaceList = new List<string>(space);
        string emptyRow = new string('.', space[0].Length);

        for (int i = rowsToExpand.Count - 1; i >= 0; i--)
        {
            spaceList.Insert(rowsToExpand[i], emptyRow);
        }

        for (int i = columnsToExpand.Count - 1; i >= 0; i--)
        {
            for (int y = 0; y < spaceList.Count; y++)
            {
                spaceList[y] = spaceList[y].Insert(columnsToExpand[i], ".");
            }
        }

        return spaceList.ToArray();
    }

    (int y, int x)[] GetGalaxyCoordinates(string[] space)
    {
        HashSet<(int y, int x)> galaxyCoordinates = new HashSet<(int y, int x)>();
        for (int y = 0; y < space.Length; y++)
        {
            for (int x = 0; x < space[y].Length; x++)
            {
                if (space[y][x] == '#') galaxyCoordinates.Add((y, x));
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
