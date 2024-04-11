#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day06Solver : public Solver
    {
    public:
        Day06Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}