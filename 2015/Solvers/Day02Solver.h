#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day02Solver : public Solver
    {
    public:
        Day02Solver(string str) : Solver(str){};
        int SolvePart1();
        int SolvePart2();
    };
}