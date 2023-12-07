namespace AdventOfCode2023;

class Day07Solver : Solver
{
    private string _puzzleInputPath = "PuzzleInputs/07.txt";
    public override string PuzzleInputPath { get => _puzzleInputPath; }

    public override long SolvePart1()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);
        List<Hand> hands = new List<Hand>();
        int totalWinnings = 0;

        foreach (string line in lines)
        {
            hands.Add(new Hand(line));
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

    public override long SolvePart2()
    {
        string[] lines = File.ReadAllLines(PuzzleInputPath);

        return 0;
    }


    // Implementing IComparable interface lets hands list be sorted by calling hands.Sort();
    class Hand : IComparable
    {
        public int[] cardIds;
        public int bid;
        public const int CARDS_IN_HAND = 5;

        public Hand(string handLine)
        {
            string[] handLineSplit = handLine.Split(' ');
            string cardsLine = handLineSplit[0];
            string bidLine = handLineSplit[1];

            cardIds = new int[CARDS_IN_HAND];
            for (int i = 0; i < CARDS_IN_HAND; i++)
            {
                char cardLabel = cardsLine[i];
                cardIds[i] = labelToStrength[cardLabel];
            }

            bid = int.Parse(bidLine);
        }

        private HandType GetHandType()
        {
            HandType currentHandType = HandType.HighCard;
            List<int> foundLabels = new List<int>();

            for (int i = 0; i < CARDS_IN_HAND; i++)
            {
                int countSameLabel = 1;

                for (int j = i + 1; j < CARDS_IN_HAND; j++)
                {
                    if (foundLabels.Contains(cardIds[j]))
                    {
                        continue;
                    }

                    if (cardIds[i] == cardIds[j])
                    {
                        countSameLabel++;
                    }
                }

                if (countSameLabel == 5 && currentHandType < HandType.FiveOfAKind) currentHandType = HandType.FiveOfAKind;
                else if (countSameLabel == 4 && currentHandType < HandType.FourOfAKind) currentHandType = HandType.FourOfAKind;
                else if (countSameLabel == 3)
                {
                    if (currentHandType == HandType.OnePair) currentHandType = HandType.FullHouse;
                    else if (currentHandType < HandType.ThreeOfAKind) currentHandType = HandType.ThreeOfAKind;
                }
                else if (countSameLabel == 2)
                {
                    if (currentHandType == HandType.ThreeOfAKind) currentHandType = HandType.FullHouse;
                    else if (currentHandType == HandType.OnePair) currentHandType = HandType.TwoPair;
                    else if (currentHandType < HandType.OnePair) currentHandType = HandType.OnePair;
                }

                // Keep track of matched labels to prevent cases like a three-of-a-kind later being identified as a pair when searching for matches from the next card onwards.
                foundLabels.Add(cardIds[i]);
            }

            return currentHandType;
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

    static Dictionary<char, int> labelToStrength = new Dictionary<char, int>
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
