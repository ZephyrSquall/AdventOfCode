#include <iostream>
#include <fstream>
#include <string>
#include <ranges>
#include <string_view>
#include <algorithm>
#include "Day06Solver.h"

int AdventOfCode2015::Day06Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/06.txt");
    std::string line;

    bool lights[1000][1000];
    for (int i = 0; i < 1000; i++)
    {
        for (int j = 0; j < 1000; j++)
        {
            lights[i][j] = false;
        }
    }

    enum class Action
    {
        On,
        Off,
        Toggle
    };

    while (getline(infile, line))
    {
        Action action;
        std::string actionless_line;

        // The strings "turn on ", "turn off " and "toggle " only differ in their 7th character ('n', 'f', and ' ').
        if (line[6] == 'n')
        {
            action = Action::On;
            actionless_line = line.substr(8);
        }
        else if (line[6] == 'f')
        {
            action = Action::Off;
            actionless_line = line.substr(9);
        }
        else
        {
            action = Action::Toggle;
            actionless_line = line.substr(7);
        }

        int start_coordinate[2];
        int end_coordinate[2];

        // Extract x-y pairs from rest of line.
        auto coordinates = actionless_line | std::ranges::views::split(std::string_view{" through "});
        for (const auto &&[index, coordinates_view] : std::views::enumerate(coordinates))
        {
            auto coordinate_numbers = coordinates_view | std::ranges::views::split(',');

            // for (int j = 0; j < 2; j++)
            for (const auto &&[inner_index, coordinate_number_view] : std::views::enumerate(coordinate_numbers))
            {

                // Convert view to string.
                std::string coordinate_number;
                std::ranges::copy(coordinate_number_view | std::ranges::views::transform([](char c)
                                                                                         { return (char)c; }),
                                  std::back_inserter(coordinate_number));

                int number = stoi(coordinate_number);

                switch (index)
                {
                case 0:
                    start_coordinate[inner_index] = number;
                    break;
                case 1:
                    end_coordinate[inner_index] = number;
                    break;
                }
            }
        }

        // Perform the corresponding action on the corresponding range.
        for (int i = start_coordinate[0]; i <= end_coordinate[0]; i++)
        {
            for (int j = start_coordinate[1]; j <= end_coordinate[1]; j++)
            {
                switch (action)
                {
                case Action::On:
                    lights[i][j] = true;
                    break;
                case Action::Off:
                    lights[i][j] = false;
                    break;
                case Action::Toggle:
                    lights[i][j] = !lights[i][j];
                    break;
                }
            }
        }
    }

    // Count lights
    int on_lights = 0;
    for (int i = 0; i < 1000; i++)
    {
        for (int j = 0; j < 1000; j++)
        {
            if (lights[i][j] == true)
                on_lights++;
        }
    }

    return on_lights;
}

int AdventOfCode2015::Day06Solver::SolvePart2()
{
    return 0;
}