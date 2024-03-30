#include <iostream>
#include <fstream>
#include <string>
#include <cmath>
#include <stdint.h>
#include <bitset>
#include <format>
#include "Day05Solver.h"

int AdventOfCode2015::Day05Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/05.txt");
    std::string line;

    int nice_strings = 0;

    while (getline(infile, line))
    {
        int vowels = 0;
        bool double_letter = false;
        bool naughty_letters = false;

        char final_letter = line[line.size() - 1];
        if (final_letter == 'a' || final_letter == 'e' || final_letter == 'i' || final_letter == 'o' || final_letter == 'u')
            vowels++;

        for (int i = 0; i < line.size() - 1; i++)
        {
            char letter = line[i];
            if (letter == 'a' || letter == 'e' || letter == 'i' || letter == 'o' || letter == 'u')
                vowels++;

            std::string letter_pair = line.substr(i, 2);
            if (letter_pair == "ab" || letter_pair == "cd" || letter_pair == "pq" || letter_pair == "xy")
            {
                naughty_letters = true;
                break;
            }

            if (letter_pair[0] == letter_pair[1])
                double_letter = true;
        }

        if (vowels >= 3 && double_letter && !naughty_letters)
            nice_strings++;
    }

    return nice_strings;
}

int AdventOfCode2015::Day05Solver::SolvePart2()
{
    std::ifstream infile("PuzzleInputs/05.txt");
    std::string line;

    int nice_strings = 0;

    while (getline(infile, line))
    {
        bool matching_letter_pair = false;
        bool letter_repeat_after_space = false;
        for (int i = 0; i < line.size() - 2; i++)
        {
            if (line[i] == line[i + 2])
                letter_repeat_after_space = true;

            std::string letter_pair = line.substr(i, 2);
            for (int j = i + 2; j < line.size() - 1; j++)
            {
                std::string inner_letter_pair = line.substr(j, 2);
                if (letter_pair == inner_letter_pair)
                    matching_letter_pair = true;
            }
        }
        if (matching_letter_pair && letter_repeat_after_space)
            nice_strings++;
    }

    return nice_strings;
}
