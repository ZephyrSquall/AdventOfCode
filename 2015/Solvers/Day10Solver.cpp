#include <iostream>
#include <fstream>
#include <string>
#include "Day10Solver.h"

int AdventOfCode2015::Day10Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/10.txt");
    std::string line;
    std::getline(infile, line);

    for (int i = 0; i < 40; i++)
    {
        std::string new_line = "";
        char current_char;
        int count = 1;
        bool first_character = true;

        for (char c : line)
        {
            if (first_character)
            {
                current_char = c;
                first_character = false;
            }
            else
            {
                if (c == current_char)
                {
                    count++;
                }
                else
                {
                    new_line += std::to_string(count);
                    new_line += current_char;

                    current_char = c;
                    count = 1;
                }
            }
        }

        new_line += std::to_string(count);
        new_line += current_char;

        line = new_line;
    }

    return line.size();
}

int AdventOfCode2015::Day10Solver::SolvePart2()
{
    return 0;
}