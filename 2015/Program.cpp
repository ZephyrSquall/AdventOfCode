#include <iostream>
#include "Solvers/Day01Solver.h"
#include "Solvers/Day02Solver.h"

using namespace std;
using namespace AdventOfCode2015;

int main()
{
    Day01Solver day01Solver;
    Day02Solver day02Solver;

    int day01Solution1 = day01Solver.SolvePart1();
    cout << day01Solution1 << endl;

    int day01Solution2 = day01Solver.SolvePart2();
    cout << day01Solution2 << endl;

    int day02Solution1 = day02Solver.SolvePart1();
    cout << day02Solution1 << endl;

    int day02Solution2 = day02Solver.SolvePart2();
    cout << day02Solution2 << endl;
    return 0;
}