#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day05Solver : public Solver
    {
    public:
        Day05Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}