namespace AdventOfCode2023;
class Day08Solver : Solver
{
    private string _puzzleTitle = "Haunted Wasteland";
    private string _puzzleInputPath = "PuzzleInputs/08.txt";
    public override string PuzzleTitle { get => _puzzleTitle; }
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override long SolvePart1()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);

        bool[] directions = GetDirections(lines);
        Dictionary<string, LeftRightPair> network = GetNetwork(lines);
        Predicate<string> isZZZ = node => node == "ZZZ";

        return GetLoopLength("AAA", directions, network, isZZZ);
    }

    // This solution carries the assumption that each starting point in the network traverses a loop
    // that contains exactly one end node, which occurs at the very end of the loop before it
    // repeats. Thus the solution is found by finding the length of each loop, and calculating the
    // lowest common multiple of all loop lengths. This is not a general solution, but to the best
    // of my knowledge, it's the intended solution because the input file was generated in a way so
    // that this assumption was true, which would be impossibly unlikely to happen by chance. I see
    // no way to solve this puzzle for the general case except brute force, which would definitely
    // take significantly longer than 15 seconds (the stated maximum amount of time any solution
    // should take). To give some justification for my assumption, I've left the unused LoopInfo
    // class in this file, which prints a lot of information I used while working on this puzzle to
    // identify these trends in the input data. This assumption is also supported by this reddit
    // post:
    // https://www.reddit.com/r/adventofcode/comments/18e6vdf/2023_day_8_part_2_an_explanation_for_why_the/
    public override long SolvePart2()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);

        bool[] directions = GetDirections(lines);
        Dictionary<string, LeftRightPair> network = GetNetwork(lines);

        string[] startingNodes = network.Keys.Where(node => node[2] == 'A').ToArray();
        List<int> loopLengths = new List<int>();
        Predicate<string> isEndingWithZ = node => node[2] == 'Z';

        // Uncomment the following line to print the logging I used.
        // GetAndPrintLoopInfo(startingNodes, directions, network);

        foreach (string node in startingNodes)
        {
            loopLengths.Add(GetLoopLength(node, directions, network, isEndingWithZ));
        }

        long lowestCommonMultiple = loopLengths.Aggregate((long)1, (a, b) => LowestCommonMultiple(a, b));

        return lowestCommonMultiple;
    }


    // false: turn left
    // true: turn right
    bool[] GetDirections(string[] lines)
    {
        List<bool> directions = new List<bool>();

        string directionsLine = lines[0];
        foreach (char character in directionsLine)
        {
            directions.Add(character == 'R');
        }

        return directions.ToArray();
    }

    Dictionary<string, LeftRightPair> GetNetwork(string[] lines)
    {
        Dictionary<string, LeftRightPair> network = new Dictionary<string, LeftRightPair>();

        for (int i = 2; i < lines.Length; i++)
        {
            string line = lines[i];
            string strippedLine = new string(line.Where(c => !(c == ' ' || c == '(' || c == ')')).ToArray());

            string[] splitLine = strippedLine.Split('=');
            string keyLine = splitLine[0];
            string[] valueLines = splitLine[1].Split(',');

            network.Add(keyLine, new LeftRightPair
            {
                left = valueLines[0],
                right = valueLines[1],
            });
        }

        return network;
    }

    static string NetworkStep(string currentNode, bool direction, Dictionary<string, LeftRightPair> network)
    {
        if (direction) return network[currentNode].right;
        else return network[currentNode].left;
    }

    int GetLoopLength(string startingNode, bool[] directions, Dictionary<string, LeftRightPair> network, Predicate<string> isEndingNode)
    {
        string currentNode = startingNode;
        bool endFound = false;
        int steps = 0;

        while (!endFound)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                currentNode = NetworkStep(currentNode, directions[i], network);
                steps++;

                if (isEndingNode(currentNode))
                {
                    endFound = true;
                    break;
                }
            }
        }

        return steps;
    }

    long LowestCommonMultiple(long a, long b)
    {
        return a * b / GreatestCommonDivisor(a, b);
    }

    // Using the Euclidean algorithm
    long GreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    class LeftRightPair
    {
        public required string left;
        public required string right;
    }


    // All following code is evidence of my logging attempts to discover how the puzzle input forms loops.
    void GetAndPrintLoopInfo(string[] startingNodes, bool[] directions, Dictionary<string, LeftRightPair> network)
    {
        void PrintArray<T>(T[] nodes)
        {
            foreach (T node in nodes)
            {
                Console.Write($"{node}, ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("--- START LOOP INFO ---");

        Console.Write("Starting nodes: ");
        PrintArray(startingNodes);

        List<LoopInfo> loopInfos = new List<LoopInfo>();

        foreach (string node in startingNodes)
        {
            loopInfos.Add(new LoopInfo(node, directions, network));
        }

        foreach (LoopInfo loopInfo in loopInfos)
        {
            Console.WriteLine();
            Console.WriteLine($"{loopInfo.searchStartNode}:");

            Console.WriteLine($"loopLength: {loopInfo.loopLength}");
            Console.WriteLine($"loopStartNode: {loopInfo.loopStartNode}");
            Console.WriteLine($"loopStartDirectionPosition: {loopInfo.loopStartDirectionPosition}");
            Console.WriteLine($"loopOffset: {loopInfo.loopOffset}");
            foreach (ZMatch zMatch in loopInfo.zMatches)
            {
                zMatch.Print();
            }
        }

        Console.WriteLine("--- END LOOP INFO ---");
    }

    class LoopInfo
    {
        public string searchStartNode;
        public long loopLength;
        public string? loopStartNode;
        public int loopStartDirectionPosition;
        public long loopOffset;
        public long loopLengthPlusOffset;
        public List<ZMatch> zMatches = new List<ZMatch>();

        public LoopInfo(string startingNode, bool[] directions, Dictionary<string, LeftRightPair> network)
        {
            this.searchStartNode = startingNode;
            List<string> visitedNodes = new List<string>([$"{startingNode}{0}"]);
            bool loopFound = false;
            string currentNode = startingNode;
            long steps = 0;

            while (!loopFound)
            {
                for (int i = 0; i < directions.Length; i++)
                {
                    currentNode = NetworkStep(currentNode, directions[i], network);
                    string visitingNode = $"{currentNode}{i}";
                    steps++;

                    if (visitedNodes.Contains(visitingNode))
                    {
                        Console.WriteLine($"Found match on {visitingNode} at step {steps}");
                        this.loopStartNode = currentNode;
                        this.loopStartDirectionPosition = i;
                        this.loopOffset = visitedNodes.IndexOf(visitingNode);
                        this.loopLength = steps - this.loopOffset;
                        this.loopLengthPlusOffset = steps;
                        loopFound = true;
                        break;
                    }

                    if (currentNode[2] == 'Z')
                    {
                        this.zMatches.Add(new ZMatch
                        {
                            step = steps,
                            node = currentNode,
                            directionPosition = i,

                        });
                    }

                    visitedNodes.Add(visitingNode);
                }
            }
            return;
        }
    }

    class ZMatch
    {
        public required long step;
        public required string node;
        public required int directionPosition;

        public void Print()
        {
            Console.WriteLine($"step: {step}, node: {node}, dirPos: {directionPosition}");
        }
    }
}
