namespace AdventOfCode2023;

class Day10Solver : Solver
{
    private string _puzzleTitle = "Pipe Maze";
    private string _puzzleInputPath = "PuzzleInputs/10.txt";
    public override string PuzzleTitle { get => _puzzleTitle; }
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    enum Direction
    {
        North,
        East,
        South,
        West,
    }

    static readonly Dictionary<char, Direction[]> pipeShape = new Dictionary<char, Direction[]>
    {
        ['|'] = [Direction.North, Direction.South],
        ['-'] = [Direction.East, Direction.West],
        ['L'] = [Direction.North, Direction.East],
        ['J'] = [Direction.North, Direction.West],
        ['7'] = [Direction.South, Direction.West],
        ['F'] = [Direction.East, Direction.South],
    };

    public override long SolvePart1()
    {
        string[] grid = File.ReadAllLines(PuzzleInputPath);

        int sy;
        int sx;
        (sy, sx) = FindS(grid);

        PipeDirection pipeDirection1 = FindStartingPipe(grid, sy, sx);

        int steps = 0;
        while (grid[pipeDirection1.y][pipeDirection1.x] != 'S')
        {
            pipeDirection1 = GetNextPipe(grid, pipeDirection1);
            steps++;
        }

        // Add 1 because loop terminates before counting the final 'S'
        steps++;

        return steps / 2;
    }

    public override long SolvePart2()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);
        return 0;
    }


    (int y, int x) FindS(string[] grid)
    {
        for (int y = 0; y < grid.Length; y++)
        {
            string gridLine = grid[y];
            for (int x = 0; x < gridLine.Length; x++)
            {
                if (gridLine[x] == 'S') return (y, x);
            }
        }

        throw new ArgumentException("Grid does not contain 'S'");
    }

    PipeDirection FindStartingPipe(string[] grid, int sy, int sx)
    {
        List<PipeDirection> pipeDirections = new List<PipeDirection>();

        // Check north
        if (sy - 1 >= 0)
        {
            char pipeNorth = grid[sy - 1][sx];
            if (pipeShape[pipeNorth].Contains(Direction.South))
            {
                pipeDirections.Add(new PipeDirection
                {
                    y = sy - 1,
                    x = sx,
                    previousDirection = Direction.North,
                });
            }
        }

        // Check east
        if (sx + 1 < grid[sy].Length)
        {
            char pipeEast = grid[sy][sx + 1];
            if (pipeShape[pipeEast].Contains(Direction.West))
            {
                pipeDirections.Add(new PipeDirection
                {
                    y = sy,
                    x = sx + 1,
                    previousDirection = Direction.East,
                });
            }
        }

        // Check south
        if (sy + 1 < grid.Length)
        {
            char pipeSouth = grid[sy + 1][sx];
            if (pipeShape[pipeSouth].Contains(Direction.North))
            {
                pipeDirections.Add(new PipeDirection
                {
                    y = sy + 1,
                    x = sx,
                    previousDirection = Direction.South,
                });
            }
        }

        // Check west
        if (sx - 1 >= 0)
        {
            char pipeWest = grid[sy][sx - 1];
            if (pipeShape[pipeWest].Contains(Direction.East))
            {
                pipeDirections.Add(new PipeDirection
                {
                    y = sy,
                    x = sx - 1,
                    previousDirection = Direction.West,
                });
            }
        }

        if (pipeDirections.Count != 2) throw new ArgumentException("'S' does not have exactly two pipes connected to it");

        return pipeDirections[0];
    }

    PipeDirection GetNextPipe(string[] grid, PipeDirection pipeDirection)
    {
        char pipe = grid[pipeDirection.y][pipeDirection.x];
        Direction[] directions = pipeShape[pipe];

        Direction flippedDirection;
        if (pipeDirection.previousDirection == Direction.North) flippedDirection = Direction.South;
        else if (pipeDirection.previousDirection == Direction.East) flippedDirection = Direction.West;
        else if (pipeDirection.previousDirection == Direction.South) flippedDirection = Direction.North;
        else flippedDirection = Direction.East;

        Direction nextDirection;
        if (directions[0] == flippedDirection) nextDirection = directions[1];
        else if (directions[1] == flippedDirection) nextDirection = directions[0];
        else throw new ArgumentException("Next pipe does not connect to this pipe");

        int yNext = pipeDirection.y;
        int xNext = pipeDirection.x;
        if (nextDirection == Direction.North) yNext--;
        else if (nextDirection == Direction.East) xNext++;
        else if (nextDirection == Direction.South) yNext++;
        else if (nextDirection == Direction.West) xNext--;

        return new PipeDirection
        {
            y = yNext,
            x = xNext,
            previousDirection = nextDirection,
        };
    }

    class PipeDirection
    {
        public int y;
        public int x;
        public Direction previousDirection;
    }
}
