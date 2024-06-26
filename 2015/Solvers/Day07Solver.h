#include <string>
#include "../Solver.h"

namespace AdventOfCode2015
{
    class Day07Solver : public Solver
    {
    public:
        Day07Solver(std::string str) : Solver(str){};
        int part_1_answer;
        std::string SolvePart1();
        std::string SolvePart2();
    };
}