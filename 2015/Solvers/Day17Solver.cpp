#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <functional>
#include "Day17Solver.h"

std::string AdventOfCode2015::Day17Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/17.txt");
    std::string line;

    std::vector<int> container_sizes = {};

    while (getline(infile, line))
    {
        container_sizes.push_back(stoi(line));
    }

    const int eggnog_total = 150;
    int container_combinations = 0;

    // Recursive function to check every possible combination of containers.
    std::function<void(int, int)> generate_combinations = [&container_combinations, container_sizes, &generate_combinations](int starting_index, int eggnog)
    {
        // If the eggnog is exactly equal to the eggnog_total, than the combination of containers
        // selected so far is a valid combination, so increment the count.
        if (eggnog == eggnog_total)
        {
            container_combinations++;
        }
        // Only continue if eggnog hasn't exceeded eggnog_total. There is no point checking if the
        // eggnog_total has been exceeded as the total capacity of the containers chosen so far
        // cannot decrease.
        else if (eggnog < eggnog_total)
        {
            // For each container, add its capacity to the eggnog amount, then call this function
            // again to repeat the process with that container for each container that comes after
            // it. (add 1 to i for the starting_index so it doesn't repeatedly use the same
            // container. This for loop won't execute any iterations if i is initially equal to
            // container_sizes.size(), so there is no risk of accessing elements outside array
            // bounds)
            for (int i = starting_index; i < container_sizes.size(); i++)
            {
                generate_combinations(i + 1, eggnog + container_sizes[i]);
            }
        }
    };

    // Start with the first index (0) with the initial amount of eggnog poured into containers (0).
    generate_combinations(0, 0);

    return std::to_string(container_combinations);
}

std::string AdventOfCode2015::Day17Solver::SolvePart2()
{
    return "0";
}