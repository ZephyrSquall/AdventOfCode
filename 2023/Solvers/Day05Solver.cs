namespace AdventOfCode2023;

class Day05Solver : Solver
{
    private string _puzzleInputPath = "PuzzleInputs/05.txt";
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override long SolvePart1()
    {
        string puzzleInput = File.ReadAllText(PuzzleInputPath);

        // Strip all alphabetical characters and dashes, which are only found in names.
        // The names of each mapping aren't important, all that matters is that sets of mappings are separated by colons, specific mappings are separated by newlines, and the numbers in each mapping are separated by spaces.
        // The seed ids themselves are identified by being the first set of numbers, and also don't need to be named.
        string strippedPuzzleInput = new string(puzzleInput.Where(c => !(Char.IsLetter(c) || c == '-')).ToArray());

        string[] mappingSetStrings = strippedPuzzleInput.Split(':', StringSplitOptions.RemoveEmptyEntries);

        string seedIdSetString = mappingSetStrings[0];
        long[] seedIds = GetNumbersFromString(seedIdSetString);

        List<Mapper> mappers = new List<Mapper>();

        for (int i = 1; i < mappingSetStrings.Length; i++)
        {
            string mappingSetString = mappingSetStrings[i];
            Mapper mapper = new Mapper(mappingSetString);
            mappers.Add(mapper);
        }

        List<long> locations = new List<long>();

        foreach (long seedId in seedIds)
        {
            long mappedValue = seedId;

            foreach (Mapper mapper in mappers)
            {
                mappedValue = mapper.Map(mappedValue);
            }

            locations.Add(mappedValue);
        }

        long lowestLocation = locations.Min();
        return lowestLocation;
    }

    public override long SolvePart2()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);

        return 0;
    }


    class Mapper
    {
        private List<Mapping> mappings = new List<Mapping>();

        public Mapper(string mappingSetString)
        {
            string[] mappingStrings = mappingSetString.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // Adding "StringSplitOptions.RemoveEmptyEntries" to Split above does not work as some lines only have spaces (due to how letters were stripped), and spaces can't be removed at this stage because they're needed to split numbers within a mapping.
            // Hence manually checking for mappings without any digits and removing them is necessary.
            string[] strippedMappingStrings = mappingStrings.Where(s => s.Any(c => Char.IsDigit(c))).ToArray();

            for (int j = 0; j < strippedMappingStrings.Length; j++)
            {
                string mappingString = strippedMappingStrings[j];
                long[] mappingValues = GetNumbersFromString(mappingString);

                Mapping mapping = new Mapping
                {
                    destination = mappingValues[0],
                    source = mappingValues[1],
                    length = mappingValues[2]
                };
                mappings.Add(mapping);
            }
        }

        public long Map(long input)
        {
            foreach (Mapping mapping in mappings)
            {
                if (input >= mapping.source && input < mapping.source + mapping.length)
                {
                    long offset = input - mapping.source;
                    return mapping.destination + offset;
                }
            }

            return input;
        }
    }

    class Mapping
    {
        public long destination;
        public long source;
        public long length;
    }

    static long[] GetNumbersFromString(string input)
    {
        string[] splitInput = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        long[] numbers = splitInput.Select(x => long.Parse(x)).ToArray();
        return numbers;
    }
}
