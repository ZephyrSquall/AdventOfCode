#include <fstream>
#include <string>
#include "Day01Solver.h"

using namespace std;
using namespace AdventOfCode2015;

int Day01Solver::SolvePart1()
{
    ifstream puzzleInput("PuzzleInputs/01.txt");
    string line;
    getline(puzzleInput, line);

    int floor = 0;

    for(int i = 0; i < line.size(); i++)
    {
        if(line[i] == '(') floor++;
        if(line[i] == ')') floor--;
    }

    return floor;
}

int Day01Solver::SolvePart2()
{
    return 0;
}