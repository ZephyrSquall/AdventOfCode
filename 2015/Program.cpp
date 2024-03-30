#include <iostream>
#include <algorithm>
#include <chrono>
#include <iomanip>
#include "Solvers/Day01Solver.h"
#include "Solvers/Day02Solver.h"
#include "Solvers/Day03Solver.h"
#include "Solvers/Day04Solver.h"
#include "Solver.h"

// "═" (the box-drawing character, not an equal sign) is stored as two characters in a char array,
// so the usual std::string(int n, char c) constructor doesn't work for repeating the "═" character
// n times. Hence a custom function is needed.
std::string repeatCharacter(int n, std::string str)
{
    std::string out = str;
    for (int i = 1; i < n; i++)
        out += str;
    return out;
}

struct DayResult
{
    std::string title;
    std::string solution1;
    std::string solution2;
    std::string time1;
    std::string time2;
};

int main()
{
    const int NUM_SOLVERS = 4;

    AdventOfCode2015::Day01Solver day01Solver("Not Quite Lisp");
    AdventOfCode2015::Day02Solver day02Solver("I Was Told There Would Be No Math");
    AdventOfCode2015::Day03Solver day03Solver("Perfectly Spherical Houses in a Vacuum");
    AdventOfCode2015::Day04Solver day04Solver("The Ideal Stocking Stuffer");

    AdventOfCode2015::Solver *solvers[NUM_SOLVERS] = {
        &day01Solver,
        &day02Solver,
        &day03Solver,
        &day04Solver,
        };

    const std::string DAY_TITLE = "Day";
    const std::string PUZZLE_TITLE = "Puzzle";
    const std::string PART_TITLE = "Part";
    const std::string SOLUTION_TITLE = "Solution";
    const std::string TIMING_TITLE = "Time (ms)";

    int maxDayLength = std::max((int)DAY_TITLE.size(), 2); // Only values for days are two-digit numbers.
    int maxPuzzleLength = PUZZLE_TITLE.size();
    int maxPartLength = std::max((int)PART_TITLE.size(), 1); // Only values for part are "1" or "2".
    int maxSolutionLength = SOLUTION_TITLE.size();
    int maxTimingLength = TIMING_TITLE.size();

    DayResult dayResults[NUM_SOLVERS];

    for (int i = 0; i < NUM_SOLVERS; i++)
    {
        auto solution1Start = std::chrono::high_resolution_clock::now();
        int solution1 = solvers[i]->SolvePart1();
        auto solution1End = std::chrono::high_resolution_clock::now();
        std::chrono::duration<double, std::milli> time1 = solution1End - solution1Start;

        auto solution2Start = std::chrono::high_resolution_clock::now();
        int solution2 = solvers[i]->SolvePart2();
        auto solution2End = std::chrono::high_resolution_clock::now();
        std::chrono::duration<double, std::milli> time2 = solution2End - solution2Start;

        dayResults[i].title = solvers[i]->puzzleTitle;
        int titleSize = dayResults[i].title.size();
        if (titleSize > maxPuzzleLength)
            maxPuzzleLength = titleSize;

        dayResults[i].solution1 = std::to_string(solution1);
        int solution1Size = dayResults[i].solution1.size();
        if (solution1Size > maxSolutionLength)
            maxSolutionLength = solution1Size;

        dayResults[i].solution2 = std::to_string(solution2);
        int solution2Size = dayResults[i].solution2.size();
        if (solution2Size > maxSolutionLength)
            maxSolutionLength = solution2Size;

        dayResults[i].time1 = std::to_string(time1.count());
        int time1Size = dayResults[i].time1.size();
        if (time1Size > maxTimingLength)
            maxTimingLength = time1Size;

        dayResults[i].time2 = std::to_string(time2.count());
        int time2Size = dayResults[i].time2.size();
        if (time2Size > maxTimingLength)
            maxTimingLength = time2Size;
    }

    // Print the table using all the results calculated thus far.
    std::cout << "╔═" << repeatCharacter(maxDayLength, "═") << "═╤═" << repeatCharacter(maxPuzzleLength, "═") << "═╤═" << repeatCharacter(maxPartLength, "═") << "═╤═" << repeatCharacter(maxSolutionLength, "═") << "═╤═" << repeatCharacter(maxTimingLength, "═") << "═╗" << std::endl;
    std::cout << "║ " << std::setw(maxDayLength) << DAY_TITLE << " │ " << std::setw(maxPuzzleLength) << PUZZLE_TITLE << " │ " << std::setw(maxPartLength) << PART_TITLE << " │ " << std::setw(maxSolutionLength) << SOLUTION_TITLE << " │ " << std::setw(maxTimingLength) << TIMING_TITLE << " ║" << std::endl;
    std::cout << "╟─" << repeatCharacter(maxDayLength, "─") << "─┼─" << repeatCharacter(maxPuzzleLength, "─") << "─┼─" << repeatCharacter(maxPartLength, "─") << "─┼─" << repeatCharacter(maxSolutionLength, "─") << "─┼─" << repeatCharacter(maxTimingLength, "─") << "─╢" << std::endl;

    bool firstRow = true;
    for (int i = 0; i < NUM_SOLVERS; i++)
    {
        if (firstRow)
        {
            firstRow = false;
        }
        else
        {
            std::cout << "║ " << std::setw(maxDayLength) << ""
                 << " │ " << std::setw(maxPuzzleLength) << ""
                 << " │ " << std::setw(maxPartLength) << ""
                 << " │ " << std::setw(maxSolutionLength) << ""
                 << " │ " << std::setw(maxTimingLength) << ""
                 << " ║" << std::endl;
        }
        std::cout << "║ " << std::setw(maxDayLength) << i + 1 << " │ " << std::setw(maxPuzzleLength) << dayResults[i].title << " │ " << std::setw(maxPartLength) << "1"
             << " │ " << std::setw(maxSolutionLength) << dayResults[i].solution1 << " │ " << std::setw(maxTimingLength) << dayResults[i].time1
             << " ║" << std::endl;
        std::cout << "║ " << std::setw(maxDayLength) << ""
             << " │ " << std::setw(maxPuzzleLength) << ""
             << " │ " << std::setw(maxPartLength) << "2"
             << " │ " << std::setw(maxSolutionLength) << dayResults[i].solution2 << " │ " << std::setw(maxTimingLength) << dayResults[i].time2
             << " ║" << std::endl;
    }

    std::cout << "╚═" << repeatCharacter(maxDayLength, "═") << "═╧═" << repeatCharacter(maxPuzzleLength, "═") << "═╧═" << repeatCharacter(maxPartLength, "═") << "═╧═" << repeatCharacter(maxSolutionLength, "═") << "═╧═" << repeatCharacter(maxTimingLength, "═") << "═╝" << std::endl;

    return 0;
}