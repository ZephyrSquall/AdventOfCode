#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day09Solver : public Solver
    {
    public:
        Day09Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}