#include <iostream>
#include <algorithm>
#include <chrono>
#include <iomanip>
#include <vector>
#include "Solvers/Day01Solver.h"
#include "Solvers/Day02Solver.h"
#include "Solvers/Day03Solver.h"
#include "Solvers/Day04Solver.h"
#include "Solvers/Day05Solver.h"
#include "Solvers/Day06Solver.h"
#include "Solvers/Day07Solver.h"
#include "Solvers/Day08Solver.h"
#include "Solvers/Day09Solver.h"
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

struct MaxLengths
{
    int maxDayLength;
    int maxPuzzleLength;
    int maxPartLength;
    int maxSolutionLength;
    int maxTimingLength;
};

struct DayResult
{
    std::string day;
    std::string title;
    std::string solution1;
    std::string solution2;
    std::string time1;
    std::string time2;
};

DayResult getResults(int day, MaxLengths &maxLengths, AdventOfCode2015::Solver *solver)
{
    DayResult dayResult{};

    auto solution1Start = std::chrono::high_resolution_clock::now();
    int solution1 = solver->SolvePart1();
    auto solution1End = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double, std::milli> time1 = solution1End - solution1Start;

    auto solution2Start = std::chrono::high_resolution_clock::now();
    int solution2 = solver->SolvePart2();
    auto solution2End = std::chrono::high_resolution_clock::now();
    std::chrono::duration<double, std::milli> time2 = solution2End - solution2Start;

    dayResult.day = std::to_string(day);

    dayResult.title = solver->puzzleTitle;
    int titleSize = dayResult.title.size();
    if (titleSize > maxLengths.maxPuzzleLength)
        maxLengths.maxPuzzleLength = titleSize;

    dayResult.solution1 = std::to_string(solution1);
    int solution1Size = dayResult.solution1.size();
    if (solution1Size > maxLengths.maxSolutionLength)
        maxLengths.maxSolutionLength = solution1Size;

    dayResult.solution2 = std::to_string(solution2);
    int solution2Size = dayResult.solution2.size();
    if (solution2Size > maxLengths.maxSolutionLength)
        maxLengths.maxSolutionLength = solution2Size;

    dayResult.time1 = std::to_string(time1.count());
    int time1Size = dayResult.time1.size();
    if (time1Size > maxLengths.maxTimingLength)
        maxLengths.maxTimingLength = time1Size;

    dayResult.time2 = std::to_string(time2.count());
    int time2Size = dayResult.time2.size();
    if (time2Size > maxLengths.maxTimingLength)
        maxLengths.maxTimingLength = time2Size;

    return dayResult;
}

int main(int argc, char *argv[])
{
    const int NUM_SOLVERS = 9;

    AdventOfCode2015::Day01Solver day01Solver("Not Quite Lisp");
    AdventOfCode2015::Day02Solver day02Solver("I Was Told There Would Be No Math");
    AdventOfCode2015::Day03Solver day03Solver("Perfectly Spherical Houses in a Vacuum");
    AdventOfCode2015::Day04Solver day04Solver("The Ideal Stocking Stuffer");
    AdventOfCode2015::Day05Solver day05Solver("Doesn't He Have Intern-Elves For This?");
    AdventOfCode2015::Day06Solver day06Solver("Probably a Fire Hazard");
    AdventOfCode2015::Day07Solver day07Solver("Some Assembly Required");
    AdventOfCode2015::Day08Solver day08Solver("Matchsticks");
    AdventOfCode2015::Day09Solver day09Solver("All in a Single Night");

    AdventOfCode2015::Solver *solvers[NUM_SOLVERS] = {
        &day01Solver,
        &day02Solver,
        &day03Solver,
        &day04Solver,
        &day05Solver,
        &day06Solver,
        &day07Solver,
        &day08Solver,
        &day09Solver,
    };

    const std::string DAY_TITLE = "Day";
    const std::string PUZZLE_TITLE = "Puzzle";
    const std::string PART_TITLE = "Part";
    const std::string SOLUTION_TITLE = "Solution";
    const std::string TIMING_TITLE = "Time (ms)";

    MaxLengths maxLengths{
        .maxDayLength = std::max((int)DAY_TITLE.size(), 2), // Only values for days are two-digit numbers.
        .maxPuzzleLength = (int)PUZZLE_TITLE.size(),
        .maxPartLength = std::max((int)PART_TITLE.size(), 1), // Only values for part are "1" or "2".
        .maxSolutionLength = (int)SOLUTION_TITLE.size(),
        .maxTimingLength = (int)TIMING_TITLE.size(),
    };

    std::vector<DayResult> dayResults;

    if (argc > 1)
    {
        for (int i = 1; i < argc; i++)
        {
            int day = std::stoi(argv[i]);
            AdventOfCode2015::Solver *solver = solvers[day - 1];
            dayResults.push_back(getResults(day, maxLengths, solver));
        }
    }
    else
    {
        for (int i = 0; i < NUM_SOLVERS; i++)
        {
            AdventOfCode2015::Solver *solver = solvers[i];
            dayResults.push_back(getResults(i + 1, maxLengths, solver));
        }
    }

    // Print the table using all the results calculated thus far.
    std::cout << "╔═" << repeatCharacter(maxLengths.maxDayLength, "═") << "═╤═" << repeatCharacter(maxLengths.maxPuzzleLength, "═") << "═╤═" << repeatCharacter(maxLengths.maxPartLength, "═") << "═╤═" << repeatCharacter(maxLengths.maxSolutionLength, "═") << "═╤═" << repeatCharacter(maxLengths.maxTimingLength, "═") << "═╗" << std::endl;
    std::cout << "║ " << std::setw(maxLengths.maxDayLength) << DAY_TITLE << " │ " << std::setw(maxLengths.maxPuzzleLength) << PUZZLE_TITLE << " │ " << std::setw(maxLengths.maxPartLength) << PART_TITLE << " │ " << std::setw(maxLengths.maxSolutionLength) << SOLUTION_TITLE << " │ " << std::setw(maxLengths.maxTimingLength) << TIMING_TITLE << " ║" << std::endl;
    std::cout << "╟─" << repeatCharacter(maxLengths.maxDayLength, "─") << "─┼─" << repeatCharacter(maxLengths.maxPuzzleLength, "─") << "─┼─" << repeatCharacter(maxLengths.maxPartLength, "─") << "─┼─" << repeatCharacter(maxLengths.maxSolutionLength, "─") << "─┼─" << repeatCharacter(maxLengths.maxTimingLength, "─") << "─╢" << std::endl;

    bool firstRow = true;
    for (int i = 0; i < dayResults.size(); i++)
    {
        if (firstRow)
        {
            firstRow = false;
        }
        else
        {
            std::cout << "║ " << std::setw(maxLengths.maxDayLength) << ""
                      << " │ " << std::setw(maxLengths.maxPuzzleLength) << ""
                      << " │ " << std::setw(maxLengths.maxPartLength) << ""
                      << " │ " << std::setw(maxLengths.maxSolutionLength) << ""
                      << " │ " << std::setw(maxLengths.maxTimingLength) << ""
                      << " ║" << std::endl;
        }
        std::cout << "║ " << std::setw(maxLengths.maxDayLength) << dayResults[i].day << " │ " << std::setw(maxLengths.maxPuzzleLength) << dayResults[i].title << " │ " << std::setw(maxLengths.maxPartLength) << "1"
                  << " │ " << std::setw(maxLengths.maxSolutionLength) << dayResults[i].solution1 << " │ " << std::setw(maxLengths.maxTimingLength) << dayResults[i].time1
                  << " ║" << std::endl;
        std::cout << "║ " << std::setw(maxLengths.maxDayLength) << ""
                  << " │ " << std::setw(maxLengths.maxPuzzleLength) << ""
                  << " │ " << std::setw(maxLengths.maxPartLength) << "2"
                  << " │ " << std::setw(maxLengths.maxSolutionLength) << dayResults[i].solution2 << " │ " << std::setw(maxLengths.maxTimingLength) << dayResults[i].time2
                  << " ║" << std::endl;
    }

    std::cout << "╚═" << repeatCharacter(maxLengths.maxDayLength, "═") << "═╧═" << repeatCharacter(maxLengths.maxPuzzleLength, "═") << "═╧═" << repeatCharacter(maxLengths.maxPartLength, "═") << "═╧═" << repeatCharacter(maxLengths.maxSolutionLength, "═") << "═╧═" << repeatCharacter(maxLengths.maxTimingLength, "═") << "═╝" << std::endl;

    return 0;
}