namespace AdventOfCode2023;

class Day02Solver : Solver
{
    private string _puzzleInputPath = "PuzzleInputs/02.txt";
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override long SolvePart1()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);
        Dictionary<string, int> cubeLimits = new Dictionary<string, int>
        {
            ["red"] = 12,
            ["green"] = 13,
            ["blue"] = 14,
        };
        int count = 0;

        foreach (string line in lines)
        {
            string[] gameAndDraws = line.Replace(" ", "").Split(":");
            string gameLine = gameAndDraws[0];
            string drawsLine = gameAndDraws[1];

            gameLine = gameLine.Replace("Game", "");
            int gameID = int.Parse(gameLine);

            bool cubeLimitsExceeded = false;

            string[] draws = drawsLine.Split(";");
            foreach (string draw in draws)
            {
                string[] cubes = draw.Split(",");
                foreach (string cube in cubes)
                {
                    bool redExceeds = ColourExceedsLimit(cube, "red", cubeLimits);
                    bool greenExceeds = ColourExceedsLimit(cube, "green", cubeLimits);
                    bool blueExceeds = ColourExceedsLimit(cube, "blue", cubeLimits);

                    if (redExceeds || greenExceeds || blueExceeds)
                    {
                        cubeLimitsExceeded = true;
                    }
                }
            }

            if (!cubeLimitsExceeded)
            {
                count += gameID;
            }
        }

        return count;
    }

    public override long SolvePart2()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);
        string[] colours = ["red", "green", "blue"];
        int count = 0;

        foreach (string line in lines)
        {
            string[] gameAndDraws = line.Replace(" ", "").Split(":");
            string drawsLine = gameAndDraws[1];

            Dictionary<string, int> cubeMinimums = new Dictionary<string, int>
            {
                ["red"] = 0,
                ["green"] = 0,
                ["blue"] = 0,
            };

            string[] draws = drawsLine.Split(";");
            foreach (string draw in draws)
            {
                string[] cubes = draw.Split(",");
                foreach (string cube in cubes)
                {
                    string cubeColour = "";
                    string cubeTrimmed = "";

                    foreach (string colour in colours)
                    {
                        if (cube.Contains(colour))
                        {
                            cubeColour = colour;
                            cubeTrimmed = cube.Replace(colour, "");
                            break;
                        }
                    }

                    int amount = int.Parse(cubeTrimmed);
                    if (cubeMinimums[cubeColour] < amount)
                    {
                        cubeMinimums[cubeColour] = amount;
                    }
                }
            }

            int power = cubeMinimums["red"] * cubeMinimums["green"] * cubeMinimums["blue"];
            count += power;
        }

        return count;
    }


    bool ColourExceedsLimit(string cube, string colour, Dictionary<string, int> cubeLimits)
    {
        if (cube.Contains(colour))
        {
            cube = cube.Replace(colour, "");
            int amount = int.Parse(cube);
            if (amount > cubeLimits[colour])
            {
                return true;
            }
        }
        return false;
    }
}
