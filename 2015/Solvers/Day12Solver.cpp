#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include "Day12Solver.h"

std::string AdventOfCode2015::Day12Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/12.txt");
    std::string line;
    std::getline(infile, line);

    std::string char_buffer = "";
    bool reading_number = false;
    int sum = 0;

    for (char c : line)
    {
        if (c == '-' || isdigit(c))
        {
            reading_number = true;
            char_buffer += c;
        }
        else if (reading_number)
        {
            sum += std::stoi(char_buffer);

            char_buffer = "";
            reading_number = false;
        }
    }

    return std::to_string(sum);
}

std::string AdventOfCode2015::Day12Solver::SolvePart2()
{
    std::ifstream infile("PuzzleInputs/12.txt");
    std::string line;
    std::getline(infile, line);

    std::string char_buffer = "";
    bool reading_number = false;
    int sum = 0;

    int current_depth = 0;
    // Keep track of depth levels that are arrays. This is necessary to remember whether we are in
    // an array or not when exiting from a deeper level.
    std::vector<int> depths_that_are_arrays = {};
    // Keep track of lowest depth that a red has been found in. If no "red" has been found in this
    // object or any of it parents, the lowest depth found will be set to "infinity" (represented by
    // INT_MAX).
    int red_depth = INT_MAX;
    // Keep track of the current sum so far at each level. This is necessary because we never know
    // whether there could be a "red" further on in the object that will invalidate the sum on the
    // current level. Upon exiting from a deeper level, current_depth will be checked against
    // red_depth to see if and "red" has been found at this level or in a parent level, and if so
    // the prospective_sum for the current level will be discarded. Otherwise, the prospective_sum
    // will be added to the level above. Nothing will be added to the overall sum until the very
    // end, as there always remains the possibility that a parent will encounter a "red" that
    // invalidates its sum too.
    std::vector<int> provisional_sums = {0};

    for (int i = 0; i < line.size(); i++)
    {
        char c = line[i];

        // Check if the current level is an array. If so, and "red"s found here will be ignored.
        bool current_object_is_array;
        if (depths_that_are_arrays.empty())
        {
            current_object_is_array = false;
        }
        else
        {
            current_object_is_array = depths_that_are_arrays.back() == current_depth;
        }

        // If the current object is not an array AND the current character is at least 5 characters
        // in AND this character and the last four spell "\"red\"".
        if (!current_object_is_array && i >= 4 && line[i - 4] == '"' && line[i - 3] == 'r' && line[i - 2] == 'e' && line[i - 1] == 'd' && c == '"')
        {
            // Only update red_depth is it is higher than the current depth. Otherwise the depth of
            // a "red" found by a parent object will be overwritten by the depth of this object.
            if (red_depth > current_depth)
            {
                red_depth = current_depth;
            }
        }

        if (current_depth < red_depth)
        {
            // Read any numbers found and add them to provisional_sums, following the same procedure
            // as in part 1.
            if (c == '-' || isdigit(c))
            {
                reading_number = true;
                char_buffer += c;
            }
            else if (reading_number)
            {
                provisional_sums[current_depth] += std::stoi(char_buffer);

                char_buffer = "";
                reading_number = false;
            }
        }

        // Entering a child object.
        if (c == '{')
        {
            current_depth++;
            provisional_sums.push_back(0);
        }
        // Entering a child array.
        else if (c == '[')
        {
            current_depth++;
            depths_that_are_arrays.push_back(current_depth);
            provisional_sums.push_back(0);
        }
        // Exiting from a child object.
        else if (c == '}')
        {
            // If red_depth is still INT_MAX, no "red" has been found in this object or any of its
            // children, so it's safe to add the current provisional_sum to the higher level.
            if (red_depth == INT_MAX)
            {
                provisional_sums[current_depth - 1] += provisional_sums[current_depth];
            }
            // Otherwise, if the current depth is equal to the red_depth, we are exiting from the
            // level with the lowest depth that contains "red" that we have found, so set red_depth
            // to INT_MAX to indicate numbers are no longer being invalidated. (If current_depth is
            // greater than red_depth, then a "red" has been found in a parent object, so red_depth
            // should remain untouched.)
            else if (current_depth <= red_depth)
            {
                red_depth = INT_MAX;
            }
            provisional_sums.pop_back();
            current_depth--;
        }
        // Exiting from a child array.
        else if (c == ']')
        {

            depths_that_are_arrays.pop_back();
            if (red_depth == INT_MAX)
            {
                provisional_sums[current_depth - 1] += provisional_sums[current_depth];
            }
            else if (current_depth <= red_depth)
            {
                red_depth = INT_MAX;
            }
            provisional_sums.pop_back();
            current_depth--;
        }
    }

    sum = provisional_sums[0];

    return std::to_string(sum);
}