#pragma once

namespace AdventOfCode2015
{
    class Solver
    {
    public:
        std::string puzzleTitle;
        Solver(std::string str) : puzzleTitle(str) {}
        virtual int SolvePart1() = 0;
        virtual int SolvePart2() = 0;
    };
}