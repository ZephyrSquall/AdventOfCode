#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day11Solver : public Solver
    {
    public:
        Day11Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}