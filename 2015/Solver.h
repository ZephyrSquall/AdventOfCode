#pragma once

using namespace std;

namespace AdventOfCode2015
{
    class Solver
    {
    public:
        string puzzleTitle;
        Solver(string str) : puzzleTitle(str) {}
        virtual int SolvePart1() = 0;
        virtual int SolvePart2() = 0;
    };
}