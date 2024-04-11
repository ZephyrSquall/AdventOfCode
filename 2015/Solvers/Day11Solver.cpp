#include <iostream>
#include <fstream>
#include <string>
#include "Day11Solver.h"

void increment_string(std::string &str)
{
    for (int i = str.size() - 1; i >= 0; i--)
    {
        if (str[i] == 'z')
        {
            str[i] = 'a';
        }
        else
        {
            // in C++, chars are a 1-byte number whose value usually indicates the character code of
            // the character they represent. All lowercase letters from 'a' to 'z' have contiguous
            // values from 97 to 122, so incrementing will always get the next character in
            // sequence (except for 'z' which would go to '{')
            str[i]++;
            break;
        }
    }
}

std::string get_next_valid_password(std::string line)
{
    bool contains_straight = false;
    bool lacks_iol = false;
    bool contains_two_pairs = false;

    while (!(contains_straight && lacks_iol && contains_two_pairs))
    {
        // Fetch the next password and reset all conditions.
        increment_string(line);
        contains_straight = false;
        lacks_iol = true;
        contains_two_pairs = false;
        int pairs = 0;

        for (int i = 0; i < line.size(); i++)
        {
            if (line[i] == 'i' || line[i] == 'o' || line[i] == 'l')
            {
                lacks_iol = false;
                break;
            }

            if (i >= 2 && line[i - 1] == line[i] - 1 && line[i - 2] == line[i] - 2)
            {
                contains_straight = true;
            }

            if (i >= 1 && line[i] == line[i - 1] && !(i >= 2 && line[i - 1] == line[i - 2]))
            {
                pairs++;
            }
        }

        if (pairs >= 2)
        {
            contains_two_pairs = true;
        }
    }

    return line;
}

std::string AdventOfCode2015::Day11Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/11.txt");
    std::string line;
    std::getline(infile, line);

    std::string password = get_next_valid_password(line);

    // Store the answer for part 1 because part 2 requires it.
    AdventOfCode2015::Day11Solver::part_1_answer = password;

    return password;
}

std::string AdventOfCode2015::Day11Solver::SolvePart2()
{
    std::string line = AdventOfCode2015::Day11Solver::part_1_answer;

    std::string password = get_next_valid_password(line);

    return password;
}