#include <iostream>
#include <fstream>
#include <string>
#include <functional>
#include "Day18Solver.h"

std::string AdventOfCode2015::Day18Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/18.txt");
    std::string line;

    auto lights = new bool[100][100]();

    for (int i = 0; i < 100; i++)
    {
        getline(infile, line);
        for (int j = 0; j < 100; j++)
        {
            // The light at (i, j) equals true if line[j] is '#' and equals false otherwise (line[j]
            // is '.').
            lights[i][j] = line[j] == '#';
        }
    }

    // Returns the number of neighboring lights that are turned on.
    std::function<int(bool(*)[100], int, int)> on_neighbors = [](bool(*previous_lights)[100], int light_i, int light_j)
    {
        int count = 0;

        // For each light in the 3x3 grid centered on the current light:
        for (int i = light_i - 1; i <= light_i + 1; i++)
        {
            for (int j = light_j - 1; j <= light_j + 1; j++)
            {
                // i >= 0 && j >= 0 && i < 100 && j < 100: Check that i and j are within the bounds
                // of the light array (do nothing if outside of the light array anything outside the
                // light array should be treated as off, which adds nothing to the count).
                // !(i == light_i && j == light_j): Ignore the light in the center of the 3x3 grid
                // as that's the current light, not one of its eight neighbors.
                // previous_lights[i][j]: Add 1 if the light at (i, j) is on (represented by true),
                // otherwise add 0 (same as doing nothing).
                if (i >= 0 && j >= 0 && i < 100 && j < 100 && !(i == light_i && j == light_j) && previous_lights[i][j])
                {
                    count++;
                }
            }
        }

        return count;
    };

    for (int steps = 0; steps < 100; steps++)
    {
        // Copy current lights to temporary array so the previous state is remembered while current
        // lights are being updated.
        auto previous_lights = new bool[100][100]();
        std::copy(&(*lights)[0], &(*lights)[0] + 100 * 100, &(*previous_lights)[0]);

        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                int count = on_neighbors(previous_lights, i, j);

                // If the light is on, turn it off if it doesn't have 2 or 3 neighbors.
                if (lights[i][j] && !(count == 2 || count == 3))
                {
                    lights[i][j] = false;
                }
                // If the light is off, turn it on if it has 3 neighbors.
                else if (!lights[i][j] && count == 3)
                {
                    lights[i][j] = true;
                }
            }
        }

        delete previous_lights;
    }

    // Count lights
    int on_lights = 0;
    for (int i = 0; i < 100; i++)
    {
        for (int j = 0; j < 100; j++)
        {
            if (lights[i][j] == true)
            {
                on_lights++;
            }
        }
    }

    delete lights;
    return std::to_string(on_lights);
}

std::string AdventOfCode2015::Day18Solver::SolvePart2()
{
    return "0";
}