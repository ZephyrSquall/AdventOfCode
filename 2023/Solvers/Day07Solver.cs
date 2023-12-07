namespace AdventOfCode2023;

class Day07Solver : Solver
{
    private string _puzzleTitle = "Camel Cards";
    private string _puzzleInputPath = "PuzzleInputs/07.txt";
    public override string PuzzleTitle { get => _puzzleTitle; }
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override long SolvePart1()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);
        return Solve(lines, false);
    }

    public override long SolvePart2()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);
        return Solve(lines, true);
    }


    int Solve(string[] lines, bool usingJokers)
    {
        List<Hand> hands = new List<Hand>();
        int totalWinnings = 0;

        foreach (string line in lines)
        {
            hands.Add(new Hand(line, usingJokers));
        }

        hands.Sort();

        for (int i = 0; i < hands.Count; i++)
        {
            Hand hand = hands[i];
            int rank = i + 1;

            int winnings = hand.bid * rank;
            totalWinnings += winnings;
        }

        return totalWinnings;
    }

    // Implementing IComparable interface lets hands list be sorted by calling hands.Sort();
    class Hand : IComparable
    {
        public int[] cardIds;
        public int bid;
        public const int CARDS_IN_HAND = 5;
        public bool usingJokers;

        public Hand(string handLine, bool usingJokers)
        {
            this.usingJokers = usingJokers;

            string[] handLineSplit = handLine.Split(' ');
            string cardsLine = handLineSplit[0];
            string bidLine = handLineSplit[1];

            cardIds = new int[CARDS_IN_HAND];
            for (int i = 0; i < CARDS_IN_HAND; i++)
            {
                char cardLabel = cardsLine[i];

                if (usingJokers) cardIds[i] = labelToStrengthWithJokers[cardLabel];
                else cardIds[i] = labelToStrength[cardLabel];
            }

            bid = int.Parse(bidLine);
        }

        private HandType GetHandType()
        {
            Dictionary<int, int> labelCount = new Dictionary<int, int>();
            int jokerCount = 0;

            for (int i = 0; i < CARDS_IN_HAND; i++)
            {
                if (usingJokers && cardIds[i] == 0) jokerCount++;
                else if (labelCount.ContainsKey(cardIds[i])) labelCount[cardIds[i]]++;
                else labelCount.Add(cardIds[i], 1);
            }

            int[] matches = labelCount.Values.ToArray();
            // If the hand has five jokers, matches will be empty, so explicitly give the array an element that it can add the five jokers to.
            if (matches.Length == 0)
            {
                matches = [0];
            }
            Array.Sort(matches);
            Array.Reverse(matches);
            // The hand value is always increased as much as possible by adding the jokers to the count of whatever label already had the highest count so far.
            matches[0] += jokerCount;

            if (matches[0] == 5) return HandType.FiveOfAKind;
            else if (matches[0] == 4) return HandType.FourOfAKind;
            else if (matches[0] == 3)
            {
                if (matches[1] == 2) return HandType.FullHouse;
                else return HandType.ThreeOfAKind;
            }
            else if (matches[0] == 2)
            {
                if (matches[1] == 2) return HandType.TwoPair;
                else return HandType.OnePair;
            }
            return HandType.HighCard;
        }

        public int CompareTo(object? obj)
        {
            if (obj is Hand otherHand)
            {
                HandType thisHandType = this.GetHandType();
                HandType otherHandType = otherHand.GetHandType();

                if (thisHandType < otherHandType) return -1;
                else if (thisHandType > otherHandType) return 1;
                else
                {
                    for (int i = 0; i < CARDS_IN_HAND; i++)
                    {
                        if (this.cardIds[i] < otherHand.cardIds[i]) return -1;
                        else if (this.cardIds[i] > otherHand.cardIds[i]) return 1;
                    }
                }
                return 0;
            }

            if (obj == null) return 1;
            throw new ArgumentException("Object is not a Hand");
        }
    }

    static readonly Dictionary<char, int> labelToStrength = new Dictionary<char, int>
    {
        ['2'] = 0,
        ['3'] = 1,
        ['4'] = 2,
        ['5'] = 3,
        ['6'] = 4,
        ['7'] = 5,
        ['8'] = 6,
        ['9'] = 7,
        ['T'] = 8,
        ['J'] = 9,
        ['Q'] = 10,
        ['K'] = 11,
        ['A'] = 12,
    };

    static readonly Dictionary<char, int> labelToStrengthWithJokers = new Dictionary<char, int>
    {
        ['J'] = 0,
        ['2'] = 1,
        ['3'] = 2,
        ['4'] = 3,
        ['5'] = 4,
        ['6'] = 5,
        ['7'] = 6,
        ['8'] = 7,
        ['9'] = 8,
        ['T'] = 9,
        ['Q'] = 10,
        ['K'] = 11,
        ['A'] = 12,
    };

    // Ordered so that the order of ints represent the enums matches the order of which hand types beat another.
    // Hence comparisons like HandType.OnePair > HandType.HighCard return true but HandType.OnePair > HandType.FiveOfAKind return false.
    enum HandType
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }
}
