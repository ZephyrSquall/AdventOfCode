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

    for (int i = 0; i < line.size(); i++)
    {
        if (line[i] == '(')
            floor++;
        if (line[i] == ')')
            floor--;
    }

    return floor;
}

int Day01Solver::SolvePart2()
{
    ifstream puzzleInput("PuzzleInputs/01.txt");
    string line;
    getline(puzzleInput, line);

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

    return firstBasementPosition;
}