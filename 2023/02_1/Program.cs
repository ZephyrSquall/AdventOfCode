namespace _02_1;

class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines(args[0]);
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

        Console.WriteLine(count);
    }

    static bool ColourExceedsLimit(string cube, string colour, Dictionary<string, int> cubeLimits)
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
