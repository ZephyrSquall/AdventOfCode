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

        // false: turn left
        // true: turn right
        List<bool> directions = new List<bool>();

        string directionsLine = lines[0];
        foreach (char character in directionsLine)
        {
            directions.Add(character == 'R');
        }

        Dictionary<string, LeftRightPair> network = new Dictionary<string, LeftRightPair>();

        for (int i = 2; i < lines.Length; i++)
        {
            string line = lines[i];
            string strippedLine = new string(line.Where(c => !(c == ' ' || c == '(' || c == ')')).ToArray());

            string[] splitLine = strippedLine.Split('=');
            string keyLine = splitLine[0];
            string[] valueLines = splitLine[1].Split(',');

            network.Add(keyLine, new LeftRightPair{
                left = valueLines[0],
                right = valueLines[1],
            });
        }

        bool endFound = false;
        string currentNode = "AAA";
        int steps = 0;

        while(!endFound)
        {
            for( int i = 0; i < directions.Count; i++)
            {
                if (directions[i]) currentNode = network[currentNode].right;
                else currentNode = network[currentNode].left;

                steps++;

                if (currentNode == "ZZZ")
                {
                    endFound = true;
                    break;
                }
            }
        }

        return steps;
    }

    public override long SolvePart2()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);

        return 0;
    }

    class LeftRightPair {
        public required string left;
        public required string right;
    }
}
