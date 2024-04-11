#include <iostream>
#include <fstream>
#include <string>
#include "Day10Solver.h"

std::string look_and_see(std::string line, int repetitions)
{
    for (int i = 0; i < repetitions; i++)
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

    return line;
}

std::string AdventOfCode2015::Day10Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/10.txt");
    std::string line;
    std::getline(infile, line);

    line = look_and_see(line, 40);

    return std::to_string(line.size());
}

std::string AdventOfCode2015::Day10Solver::SolvePart2()
{
    std::ifstream infile("PuzzleInputs/10.txt");
    std::string line;
    std::getline(infile, line);

    line = look_and_see(line, 50);

    return std::to_string(line.size());
}