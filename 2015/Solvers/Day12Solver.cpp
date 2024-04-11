#include <iostream>
#include <fstream>
#include <string>
#include "Day12Solver.h"

std::string AdventOfCode2015::Day12Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/12.txt");
    std::string line;
    std::getline(infile, line);

    std::string char_buffer = "";
    bool reading_number = false;
    int sum = 0;

    for (char c : line)
    {
        if (c == '-' || isdigit(c))
        {
            reading_number = true;
            char_buffer += c;
        }
        else if (reading_number)
        {
            sum += std::stoi(char_buffer);

            char_buffer = "";
            reading_number = false;
        }
    }

    return std::to_string(sum);
}

std::string AdventOfCode2015::Day12Solver::SolvePart2()
{
    return "0";
}