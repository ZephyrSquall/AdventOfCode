#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day18Solver : public Solver
    {
    public:
        Day18Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}