#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day03Solver : public Solver
    {
    public:
        Day03Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}