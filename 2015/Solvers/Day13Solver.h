#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day13Solver : public Solver
    {
    public:
        Day13Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}