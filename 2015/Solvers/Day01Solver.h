#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day01Solver : public Solver
    {
    public:
        Day01Solver(string str) : Solver(str){};
        int SolvePart1();
        int SolvePart2();
    };
}