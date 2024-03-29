#include "Day02Solver.h"
#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <algorithm>

using namespace std;
using namespace AdventOfCode2015;

int Day02Solver::SolvePart1()
{
    ifstream infile("PuzzleInputs/02.txt");
    string line;
    int totalArea = 0;

    while (getline(infile, line))
    {
        istringstream lineStream(line);
        string dimensionString;
        int dimensions[3];

        for (int i = 0; i < 3; i++)
        {
            getline(lineStream, dimensionString, 'x');
            dimensions[i] = stoi(dimensionString);
        }

        int sideAreas[3] = {dimensions[0] * dimensions[1], dimensions[0] * dimensions[2], dimensions[1] * dimensions[2]};
        int slackArea = *min_element(sideAreas, sideAreas + 3);
        int wrappingPaperArea = 2 * sideAreas[0] + 2 * sideAreas[1] + 2 * sideAreas[2] + slackArea;

        totalArea += wrappingPaperArea;
    }

    return totalArea;
}

int Day02Solver::SolvePart2()
{
    return 0;
}