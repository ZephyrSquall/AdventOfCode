#include <fstream>
#include <string>
#include "Day01Solver.h"

std::string AdventOfCode2015::Day01Solver::SolvePart1()
{
    std::ifstream puzzleInput("PuzzleInputs/01.txt");
    std::string line;
    std::getline(puzzleInput, line);

    int floor = 0;

    for (int i = 0; i < line.size(); i++)
    {
        if (line[i] == '(')
            floor++;
        if (line[i] == ')')
            floor--;
    }

    return std::to_string(floor);
}

std::string AdventOfCode2015::Day01Solver::SolvePart2()
{
    std::ifstream puzzleInput("PuzzleInputs/01.txt");
    std::string line;
    std::getline(puzzleInput, line);

    int floor = 0;
    int firstBasementPosition = 0;

    for (int i = 0; i < line.size(); i++)
    {
        if (line[i] == '(')
            floor++;
        if (line[i] == ')')
            floor--;

        if (floor < 0)
        {
            // Add 1 because string index starts at 0 and character position starts at 1.
            firstBasementPosition = i + 1;
            break;
        }
    }

    return std::to_string(firstBasementPosition);
}