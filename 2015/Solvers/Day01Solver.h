#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day01Solver : public Solver
    {
    public:
        Day01Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}