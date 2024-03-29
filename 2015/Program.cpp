#include <iostream>
#include <functional>
#include <algorithm>
#include <iomanip>
#include "Solvers/Day01Solver.h"
#include "Solvers/Day02Solver.h"
#include "Solver.h"
#define UNICODE

using namespace std;
using namespace AdventOfCode2015;

// "═" (the box-drawing character, not an equal sign) is stored as two characters in a char array,
// so the usual std::string(int n, char c) constructor doesn't work for repeating the "═" character
// n times. Hence a custom function is needed.
string repeatCharacter(int n, string str)
{
    string out = str;
    for (int i = 1; i < n; i++)
        out += str;
    return out;
}

struct DayResult
{
    string title;
    string solution1;
    string solution2;
};

int main()
{
    const int NUM_SOLVERS = 2;

    Day01Solver day01Solver;
    Day02Solver day02Solver;

    Solver *solvers[NUM_SOLVERS] = {&day01Solver, &day02Solver};

    const string DAY_TITLE = "Day";
    const string PUZZLE_TITLE = "Puzzle";
    const string PART_TITLE = "Part";
    const string SOLUTION_TITLE = "Solution";
    const string TIMING_TITLE = "Time (ms)";

    int maxDayLength = max((int)DAY_TITLE.size(), 2); // Only values for days are two-digit numbers.
    int maxPuzzleLength = PUZZLE_TITLE.size();
    int maxPartLength = max((int)PART_TITLE.size(), 1); // Only values for part are "1" or "2".
    int maxSolutionLength = SOLUTION_TITLE.size();
    int maxTimingLength = TIMING_TITLE.size();

    DayResult dayResults[NUM_SOLVERS];

    for (int i = 0; i < NUM_SOLVERS; i++)
    {
        dayResults[i].title = "test title";
        int titleSize = dayResults[i].title.size();
        if (titleSize > maxPuzzleLength)
            maxPuzzleLength = titleSize;

        int solution1 = solvers[i]->SolvePart1();
        dayResults[i].solution1 = to_string(solution1);
        int solution1Size = dayResults[i].solution1.size();
        if (solution1Size > maxSolutionLength)
            maxSolutionLength = solution1Size;

        int solution2 = solvers[i]->SolvePart2();
        dayResults[i].solution2 = to_string(solution2);
        int solution2Size = dayResults[i].solution2.size();
        if (solution2Size > maxSolutionLength)
            maxSolutionLength = solution2Size;
    }

    // Print the table using all the results calculated thus far.
    cout << "╔═" << repeatCharacter(maxDayLength, "═") << "═╤═" << repeatCharacter(maxPuzzleLength, "═") << "═╤═" << repeatCharacter(maxPartLength, "═") << "═╤═" << repeatCharacter(maxSolutionLength, "═") << "═╤═" << repeatCharacter(maxTimingLength, "═") << "═╗" << endl;
    cout << "║ " << setw(maxDayLength) << DAY_TITLE << " │ " << setw(maxPuzzleLength) << PUZZLE_TITLE << " │ " << setw(maxPartLength) << PART_TITLE << " │ " << setw(maxSolutionLength) << SOLUTION_TITLE << " │ " << setw(maxTimingLength) << TIMING_TITLE << " ║" << endl;
    cout << "╟─" << repeatCharacter(maxDayLength, "─") << "─┼─" << repeatCharacter(maxPuzzleLength, "─") << "─┼─" << repeatCharacter(maxPartLength, "─") << "─┼─" << repeatCharacter(maxSolutionLength, "─") << "─┼─" << repeatCharacter(maxTimingLength, "─") << "─╢" << endl;

    bool firstRow = true;
    for (int i = 0; i < NUM_SOLVERS; i++)
    {
        if (firstRow)
        {
            firstRow = false;
        }
        else
        {
            cout << "║ " << setw(maxDayLength) << ""
                 << " │ " << setw(maxPuzzleLength) << ""
                 << " │ " << setw(maxPartLength) << ""
                 << " │ " << setw(maxSolutionLength) << ""
                 << " │ " << setw(maxTimingLength) << ""
                 << " ║" << endl;
        }
        cout << "║ " << setw(maxDayLength) << i + 1 << " │ " << setw(maxPuzzleLength) << dayResults[i].title << " │ " << setw(maxPartLength) << "1"
             << " │ " << setw(maxSolutionLength) << dayResults[i].solution1 << " │ " << setw(maxTimingLength) << "test time"
             << " ║" << endl;
        cout << "║ " << setw(maxDayLength) << ""
             << " │ " << setw(maxPuzzleLength) << ""
             << " │ " << setw(maxPartLength) << "2"
             << " │ " << setw(maxSolutionLength) << dayResults[i].solution2 << " │ " << setw(maxTimingLength) << "test time"
             << " ║" << endl;
    }

    cout << "╚═" << repeatCharacter(maxDayLength, "═") << "═╧═" << repeatCharacter(maxPuzzleLength, "═") << "═╧═" << repeatCharacter(maxPartLength, "═") << "═╧═" << repeatCharacter(maxSolutionLength, "═") << "═╧═" << repeatCharacter(maxTimingLength, "═") << "═╝" << endl;

    return 0;
}
