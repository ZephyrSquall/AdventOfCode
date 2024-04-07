#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day07Solver : public Solver
    {
    public:
        Day07Solver(std::string str) : Solver(str){};
        int SolvePart1();
        int SolvePart2();
    };
}