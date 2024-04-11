#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day10Solver : public Solver
    {
    public:
        Day10Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}