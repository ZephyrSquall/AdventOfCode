#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day16Solver : public Solver
    {
    public:
        Day16Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}