namespace _01_1;

class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines(args[0]);

        int count = 0;

        foreach (string line in lines)
        {
            bool foundFirstDigit = false;
            int firstDigit = -1;
            int lastDigit = -1;

            foreach (char c in line)
            {
                if (Char.IsDigit(c))
                {
                    int value = (int)Char.GetNumericValue(c);
                    lastDigit = value;
                    if (!foundFirstDigit)
                    {
                        firstDigit = value;
                        foundFirstDigit = true;
                    }
                }
            }

            int calibrationValue = 10 * firstDigit + lastDigit;
            count += calibrationValue;
        }

        Console.WriteLine(count);
    }
}
