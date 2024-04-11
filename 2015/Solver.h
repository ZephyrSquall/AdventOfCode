#pragma once

namespace AdventOfCode2015
{
    class Solver
    {
    public:
        std::string puzzleTitle;
        Solver(std::string str) : puzzleTitle(str) {}
        virtual std::string SolvePart1() = 0;
        virtual std::string SolvePart2() = 0;
    };
}