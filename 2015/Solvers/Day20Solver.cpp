#include <iostream>
#include <fstream>
#include <string>
#include "Day20Solver.h"

std::string AdventOfCode2015::Day20Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/20.txt");
    std::string line;
    getline(infile, line);
    int target_presents = stoi(line);

    // A house will ultimately be visited by each elf whose number evenly divides into that house's
    // number, then that elf delivers 10 times their own elf number of presents to that house.
    // Therefore each house will receive a number of presents equal to 10 times the sum of its
    // factors. So check every house in order and compute this amount of presents.

    // House 1 is the first house, but it is more convenient to increment house at the start of the
    // loop, so start house at 0 so that it is incremented to 1 for the first loop.
    int house = 0;
    int current_presents = 0;

    while (current_presents < target_presents)
    {
        house++;
        current_presents = 0;
        for (int i = 1; i <= house; i++)
        {
            // i is a factor of house if house divided by i has a remainder of 0.
            if (house % i == 0)
            {
                current_presents += i * 10;
            }
        }
    }

    return std::to_string(house);
}

std::string AdventOfCode2015::Day20Solver::SolvePart2()
{
    return "0";
}