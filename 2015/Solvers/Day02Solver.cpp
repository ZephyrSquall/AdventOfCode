#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <algorithm>
#include "Day02Solver.h"

std::string AdventOfCode2015::Day02Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/02.txt");
    std::string line;
    int totalArea = 0;

    while (getline(infile, line))
    {
        std::istringstream lineStream(line);
        std::string dimensionString;
        int dimensions[3];

        for (int i = 0; i < 3; i++)
        {
            std::getline(lineStream, dimensionString, 'x');
            dimensions[i] = stoi(dimensionString);
        }

        int sideAreas[3] = {dimensions[0] * dimensions[1], dimensions[0] * dimensions[2], dimensions[1] * dimensions[2]};
        int slackArea = *std::min_element(sideAreas, sideAreas + 3);
        int wrappingPaperArea = 2 * sideAreas[0] + 2 * sideAreas[1] + 2 * sideAreas[2] + slackArea;

        totalArea += wrappingPaperArea;
    }

    return std::to_string(totalArea);
}

std::string AdventOfCode2015::Day02Solver::SolvePart2()
{
    std::ifstream infile("PuzzleInputs/02.txt");
    std::string line;
    int totalLength = 0;

    while (std::getline(infile, line))
    {
        std::istringstream lineStream(line);
        std::string dimensionString;
        int dimensions[3];

        for (int i = 0; i < 3; i++)
        {
            std::getline(lineStream, dimensionString, 'x');
            dimensions[i] = stoi(dimensionString);
        }

        int sidePerimeters[3] = {2 * (dimensions[0] + dimensions[1]), 2 * (dimensions[0] + dimensions[2]), 2 * (dimensions[1] + dimensions[2])};
        int ribbonWrapLength = *std::min_element(sidePerimeters, sidePerimeters + 3);
        int ribbonBowLength = dimensions[0] * dimensions[1] * dimensions[2];
        int ribbonLength = ribbonWrapLength + ribbonBowLength;

        totalLength += ribbonLength;
    }

    return std::to_string(totalLength);
}