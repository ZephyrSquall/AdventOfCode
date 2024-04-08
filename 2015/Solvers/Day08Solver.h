#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day08Solver : public Solver
    {
    public:
        Day08Solver(std::string str) : Solver(str){};
        int SolvePart1();
        int SolvePart2();
    };
}