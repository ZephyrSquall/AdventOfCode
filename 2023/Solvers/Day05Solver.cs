namespace AdventOfCode2023;

class Day05Solver : Solver
{
    private string _puzzleInputPath = "PuzzleInputs/05.txt";
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override long SolvePart1()
    {
        string puzzleInput = File.ReadAllText(PuzzleInputPath);
        GetSeedIdsAndMappers(puzzleInput, out long[] seedIds, out List<Mapper> mappers);

        foreach (Mapper mapper in mappers)
        {
            seedIds = mapper.Map(seedIds);
        }

        long lowestLocation = seedIds.Min();
        return lowestLocation;
    }

    public override long SolvePart2()
    {
        string puzzleInput = File.ReadAllText(PuzzleInputPath);
        GetSeedIdsAndMappers(puzzleInput, out long[] seedIds, out List<Mapper> mappers);

        List<IdRange> seedIdRanges = new List<IdRange>();
        for (int i = 0; i < seedIds.Length; i += 2)
        {
            IdRange seedIdRange = new IdRange
            {
                start = seedIds[i],
                length = seedIds[i + 1]
            };
            seedIdRanges.Add(seedIdRange);
        }

        foreach (Mapper mapper in mappers)
        {
            seedIdRanges = mapper.Map(seedIdRanges);
        }

        // Gets the minimum starting id of all seedIds, which at this point have been fully mapped to location IDs (the starting id of a range is the smallest id of that range so there's no need to check a range's length).
        long minimumLocation = seedIdRanges.Aggregate(long.MaxValue, (currentMinimum, seedIdRange) => Math.Min(currentMinimum, seedIdRange.start));
        return minimumLocation;
    }


    class Mapper
    {
        private List<Mapping> mappings = new List<Mapping>();

        // Constructor handles the set-up of mappings that are then used when calling its Map method.
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

        public long[] Map(long[] inputs)
        {
            // Apply Map(long input) overload to each individual element in the array.
            long[] mappedInputs = inputs.Select(Map).ToArray();
            return mappedInputs;
        }

        public List<IdRange> Map(List<IdRange> inputRanges)
        {
            List<IdRange> mappedInputs = new List<IdRange>();
            Stack<IdRange> unmappedInputs = new Stack<IdRange>(inputRanges);

            while (unmappedInputs.Count != 0)
            {
                IdRange input = unmappedInputs.Pop();
                bool inputMapped = false;

                foreach (Mapping mapping in mappings)
                {
                    long inputStart = input.start;
                    long inputEnd = input.start + input.length - 1;
                    long mappingStart = mapping.source;
                    long mappingEnd = mapping.source + mapping.length - 1;

                    // If the end of the input is before the start of the mapping source, or the start of the input is after the end of the mapping source, then none of the range overlaps so skip to the next mapping.
                    if (inputStart > mappingEnd || inputEnd < mappingStart)
                    {
                        continue;
                    }

                    if (inputStart < mappingStart)
                    {
                        // Add the range of inputs from before the start of the mapping region back into the stack of unmapped ID ranges.
                        long newIdRangeStart = inputStart;
                        long newIdRangeLength = mappingStart - inputStart;
                        unmappedInputs.Push(new IdRange
                        {
                            start = newIdRangeStart,
                            length = newIdRangeLength,
                        });

                        // Adjust inputStart so only the section of the input range that falls within the mapping region is mapped.
                        inputStart = mappingStart;
                    }

                    if (inputEnd > mappingEnd)
                    {
                        // Add the range of inputs from after the end of the mapping region back into the stack of unmapped ID ranges.
                        long newIdRangeStart = mappingEnd + 1;
                        long newIdRangeLength = inputEnd - mappingEnd;
                        unmappedInputs.Push(new IdRange
                        {
                            start = newIdRangeStart,
                            length = newIdRangeLength,
                        });

                        // Adjust inputEnd so only the section of the input range that falls within the mapping region is mapped.
                        inputEnd = mappingEnd;
                    }

                    // Now that the input range has been truncated to lie entirely within the mapping range, perform the mapping on the truncated input range.
                    long mappedInputLength = inputEnd - inputStart + 1;
                    long startOffset = inputStart - mappingStart;
                    long mappedInputStart = mapping.destination + startOffset;
                    mappedInputs.Add(new IdRange
                    {
                        start = mappedInputStart,
                        length = mappedInputLength,
                    });

                    // A successful mapping was found, so stop searching for more mappings from the current input.
                    // The next loop iteration will check the unmappedInputs stack in case there were regions of the current input that were unmapped, and try to map only those regions again.
                    inputMapped = true;
                    break;
                }

                // If inputMapped is still false, this means none of the mappings matched any part of this input range, which means this entire input range maps to itself.
                if (!inputMapped)
                {
                    mappedInputs.Add(input);
                }
            }

            return mappedInputs;
        }
    }

    class Mapping
    {
        public long destination;
        public long source;
        public long length;
    }

    class IdRange
    {
        public long start;
        public long length;
    }

    void GetSeedIdsAndMappers(string puzzleInput, out long[] seedIds, out List<Mapper> mappers)
    {
        // Strip all alphabetical characters and dashes, which are only found in names.
        // The names of each mapping aren't important, all that matters is that sets of mappings are separated by colons, specific mappings are separated by newlines, and the numbers in each mapping are separated by spaces.
        // The seed ids themselves are identified by being the first set of numbers, and also don't need to be named.
        string strippedPuzzleInput = new string(puzzleInput.Where(c => !(Char.IsLetter(c) || c == '-')).ToArray());

        string[] mappingSetStrings = strippedPuzzleInput.Split(':', StringSplitOptions.RemoveEmptyEntries);

        string seedIdSetString = mappingSetStrings[0];
        seedIds = GetNumbersFromString(seedIdSetString);

        mappers = new List<Mapper>();

        for (int i = 1; i < mappingSetStrings.Length; i++)
        {
            string mappingSetString = mappingSetStrings[i];
            Mapper mapper = new Mapper(mappingSetString);
            mappers.Add(mapper);
        }
    }

    static long[] GetNumbersFromString(string input)
    {
        string[] splitInput = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        long[] numbers = splitInput.Select(x => long.Parse(x)).ToArray();
        return numbers;
    }
}
