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
    std::ifstream infile("PuzzleInputs/17.txt");
    std::string line;

    std::vector<int> container_sizes = {};

    while (getline(infile, line))
    {
        container_sizes.push_back(stoi(line));
    }

    const int eggnog_total = 150;
    int container_combinations = 0;
    int minimum_containers = INT_MAX;

    // Recursive function to check every possible combination of containers.
    std::function<void(int, int, int)> generate_combinations = [&container_combinations, &minimum_containers, container_sizes, &generate_combinations](int starting_index, int eggnog, int containers)
    {
        // If the eggnog is exactly equal to the eggnog_total, than the combination of containers
        // selected so far is a valid combination, so check if the count can be incremented.
        if (eggnog == eggnog_total)
        {
            // If the number of containers used is smaller than the previously-found minimum, then
            // update minimum_containers, then reset container_combinations as all previously-found
            // combinations weren't actually for the smallest possible number of containers (set it
            // to 1 instead of 0 to include this combination in the count)
            if (containers < minimum_containers)
            {
                minimum_containers = containers;
                container_combinations = 1;
            }
            // Otherwise, only increment the count if this combination also uses the
            // previously-found minimum number of containers.
            else if (containers == minimum_containers)
            {
                container_combinations++;
            }
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
                generate_combinations(i + 1, eggnog + container_sizes[i], containers + 1);
            }
        }
    };

    // Start with the first index (0) with the initial amount of eggnog poured into containers (0)
    // and the initial amount of containers used (0).
    generate_combinations(0, 0, 0);

    return std::to_string(container_combinations);
}