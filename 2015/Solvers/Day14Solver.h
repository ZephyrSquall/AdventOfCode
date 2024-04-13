#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day14Solver : public Solver
    {
    public:
        Day14Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}