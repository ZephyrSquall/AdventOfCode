#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day17Solver : public Solver
    {
    public:
        Day17Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}