namespace _02_2;

class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines(args[0]);
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
                    if (cubeMinimums[cubeColour] < amount) {
                        cubeMinimums[cubeColour] = amount;
                    }
                }
            }

            int power = cubeMinimums["red"] * cubeMinimums["green"] * cubeMinimums["blue"];
            count += power;
        }

        Console.WriteLine(count);
    }
}
