#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day04Solver : public Solver
    {
    public:
        Day04Solver(std::string str) : Solver(str){};
        int SolvePart1();
        int SolvePart2();
    };
}