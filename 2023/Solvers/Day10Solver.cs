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
        ['.'] = [],
    };

    public override long SolvePart1()
    {
        string[] grid = File.ReadAllLines(PuzzleInputPath);

        int sy;
        int sx;
        (sy, sx) = FindS(grid);

        PipeDirection pipeDirection = FindStartingPipe(grid, sy, sx);

        int steps = 0;
        while (grid[pipeDirection.y][pipeDirection.x] != 'S')
        {
            pipeDirection = GetNextPipe(grid, pipeDirection);
            steps++;
        }

        // Add 1 because loop terminates before counting the final 'S'
        steps++;

        return steps / 2;
    }

    public override long SolvePart2()
    {
        string[] grid = File.ReadAllLines(PuzzleInputPath);

        int sy;
        int sx;
        (sy, sx) = FindS(grid);

        PipeDirection pipeDirection = FindStartingPipe(grid, sy, sx);

        int clockwiseTurns = 0;
        HashSet<PipeDirection> loopPipes = new HashSet<PipeDirection>([pipeDirection]);

        while (grid[pipeDirection.y][pipeDirection.x] != 'S')
        {
            Direction enteringDirection = pipeDirection.previousDirection;
            pipeDirection = GetNextPipe(grid, pipeDirection);
            loopPipes.Add(pipeDirection);
            Direction leavingDirection = pipeDirection.previousDirection;

            clockwiseTurns += TurnDirection(enteringDirection, leavingDirection);
        }

        bool clockwiseLoop = clockwiseTurns > 0;
        HashSet<(int y, int x)> possibleInsideLoopCoordinates = new HashSet<(int y, int x)>();
        HashSet<(int y, int x)> insideLoopCoordinates = new HashSet<(int y, int x)>();

        // Find initial points of inside loop co-ordinates by getting points adjacent to the loop.
        foreach (PipeDirection loopPipe in loopPipes)
        {
            char pipe = grid[loopPipe.y][loopPipe.x];
            {
                if (clockwiseLoop)
                {
                    if (pipe == '|')
                    {
                        if (loopPipe.previousDirection == Direction.North) possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x + 1));
                        if (loopPipe.previousDirection == Direction.South) possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x - 1));
                    }
                    else if (pipe == '-')
                    {
                        if (loopPipe.previousDirection == Direction.East) possibleInsideLoopCoordinates.Add((loopPipe.y + 1, loopPipe.x));
                        if (loopPipe.previousDirection == Direction.West) possibleInsideLoopCoordinates.Add((loopPipe.y - 1, loopPipe.x));
                    }
                    else if (pipe == 'L' && loopPipe.previousDirection == Direction.South)
                    {
                        possibleInsideLoopCoordinates.Add((loopPipe.y+1, loopPipe.x));
                        possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x-1));
                    }
                    else if (pipe == 'J' && loopPipe.previousDirection == Direction.East)
                    {
                        possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x+1));
                        possibleInsideLoopCoordinates.Add((loopPipe.y+1, loopPipe.x));
                    }
                    else if (pipe == '7' && loopPipe.previousDirection == Direction.North)
                    {
                        possibleInsideLoopCoordinates.Add((loopPipe.y-1, loopPipe.x));
                        possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x+1));
                    }
                    else if (pipe == 'F' && loopPipe.previousDirection == Direction.West)
                    {
                        possibleInsideLoopCoordinates.Add((loopPipe.y-1, loopPipe.x));
                        possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x-1));
                    }
                }
                // if !clockwiseLoop
                else
                {
                    if (pipe == '|')
                    {
                        if (loopPipe.previousDirection == Direction.North) possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x - 1));
                        if (loopPipe.previousDirection == Direction.South) possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x + 1));
                    }
                    else if (pipe == '-')
                    {
                        if (loopPipe.previousDirection == Direction.East) possibleInsideLoopCoordinates.Add((loopPipe.y - 1, loopPipe.x));
                        if (loopPipe.previousDirection == Direction.West) possibleInsideLoopCoordinates.Add((loopPipe.y + 1, loopPipe.x));
                    }
                    else if (pipe == 'L' && loopPipe.previousDirection == Direction.West)
                    {
                        possibleInsideLoopCoordinates.Add((loopPipe.y+1, loopPipe.x));
                        possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x-1));
                    }
                    else if (pipe == 'J' && loopPipe.previousDirection == Direction.South)
                    {
                        possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x+1));
                        possibleInsideLoopCoordinates.Add((loopPipe.y+1, loopPipe.x));
                    }
                    else if (pipe == '7' && loopPipe.previousDirection == Direction.East)
                    {
                        possibleInsideLoopCoordinates.Add((loopPipe.y-1, loopPipe.x));
                        possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x+1));
                    }
                    else if (pipe == 'F' && loopPipe.previousDirection == Direction.North)
                    {
                        possibleInsideLoopCoordinates.Add((loopPipe.y-1, loopPipe.x));
                        possibleInsideLoopCoordinates.Add((loopPipe.y, loopPipe.x-1));
                    }
                }

            }
        }

        insideLoopCoordinates = PrunePossibleInsideLoopCoordinates(loopPipes, possibleInsideLoopCoordinates, insideLoopCoordinates);

        // Search for the rest of the area inside the loop by spreading out from the initial points until no more area is gained.
        int previousInsideArea = insideLoopCoordinates.Count();
        int nextInsideArea = 0;
        while (previousInsideArea != nextInsideArea)
        {
            previousInsideArea = insideLoopCoordinates.Count();
            possibleInsideLoopCoordinates = GetNewPossibleInsideLoopCoordinates(insideLoopCoordinates);
            possibleInsideLoopCoordinates = PrunePossibleInsideLoopCoordinates(loopPipes, possibleInsideLoopCoordinates, insideLoopCoordinates);
            insideLoopCoordinates.UnionWith(possibleInsideLoopCoordinates);
            nextInsideArea = insideLoopCoordinates.Count();
        }

        return nextInsideArea;
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

    // -1: Counterclockwise turn
    // 0: No turn (went straight)
    // 1: Clockwise turn
    int TurnDirection(Direction enteringDirection, Direction leavingDirection)
    {
        // enteringDirection is the direction you're travelling as you enter (so if it's south, means you're coming from the north).
        // leavingDirection is the direction you're travelling as you leave (so if it's east, means you're going to the east).
        if (enteringDirection == Direction.North)
        {
            if (leavingDirection == Direction.West) return -1;
            if (leavingDirection == Direction.East) return 1;
        }
        if (enteringDirection == Direction.East)
        {
            if (leavingDirection == Direction.North) return -1;
            if (leavingDirection == Direction.South) return 1;
        }
        if (enteringDirection == Direction.South)
        {
            if (leavingDirection == Direction.East) return -1;
            if (leavingDirection == Direction.West) return 1;
        }
        if (enteringDirection == Direction.West)
        {
            if (leavingDirection == Direction.South) return -1;
            if (leavingDirection == Direction.North) return 1;
        }
        return 0;
    }

    HashSet<(int y, int x)> PrunePossibleInsideLoopCoordinates(HashSet<PipeDirection> loopPipes, HashSet<(int y, int x)> possibleInsideLoopCoordinates, HashSet<(int y, int x)> insideLoopCoordinates)
    {
        HashSet<(int y, int x)> insideLoopCoordinatesToRemove = new HashSet<(int y, int x)>();

        foreach ((int y, int x) in possibleInsideLoopCoordinates)
        {
            if (loopPipes.Any(loopPipe => (loopPipe.y, loopPipe.x) == (y, x)) || insideLoopCoordinates.Contains((y, x)))
            {
                insideLoopCoordinatesToRemove.Add((y, x));
            }
        }

        possibleInsideLoopCoordinates.ExceptWith(insideLoopCoordinatesToRemove);
        return possibleInsideLoopCoordinates;
    }

    HashSet<(int y, int x)> GetNewPossibleInsideLoopCoordinates(HashSet<(int y, int x)> insideLoopCoordinates)
    {
        HashSet<(int y, int x)> possibleInsideLoopCoordinates = new HashSet<(int y, int x)>();

        foreach ((int y, int x) in insideLoopCoordinates)
        {
            possibleInsideLoopCoordinates.Add((y - 1, x));
            possibleInsideLoopCoordinates.Add((y, x + 1));
            possibleInsideLoopCoordinates.Add((y + 1, x));
            possibleInsideLoopCoordinates.Add((y, x - 1));
        }

        return possibleInsideLoopCoordinates;
    }

    class PipeDirection
    {
        public int y;
        public int x;
        public Direction previousDirection;
    }
}
