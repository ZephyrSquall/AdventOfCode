#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day02Solver : public Solver
    {
    public:
        Day02Solver(std::string str) : Solver(str){};
        std::string SolvePart1();
        std::string SolvePart2();
    };
}