#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day20Solver : public Solver
    {
    public:
        Day20Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}