#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include "Day03Solver.h"

int AdventOfCode2015::Day03Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/03.txt");
    std::string line;
    std::getline(infile, line);

    struct coordinate
    {
        int x;
        int y;
    };

    int current_x = 0;
    int current_y = 0;

    std::vector<coordinate> coordinates;
    coordinates.push_back(coordinate{.x = current_x, .y = current_y});

    int housesVisited = 1;

    for (int i = 0; i < line.size(); i++)
    {
        char instruction = line[i];

        if (instruction == '^')
            current_y++;
        if (instruction == 'v')
            current_y--;
        if (instruction == '>')
            current_x++;
        if (instruction == '<')
            current_x--;

        bool visitedHouse = false;

        for (int j = 0; j < coordinates.size(); j++)
        {
            if (coordinates[j].x == current_x && coordinates[j].y == current_y)
            {
                visitedHouse = true;
                break;
            }
        }

        if (!visitedHouse)
        {
            housesVisited++;
            coordinates.push_back(coordinate{.x = current_x, .y = current_y});
        }
    }

    return housesVisited;
}

int AdventOfCode2015::Day03Solver::SolvePart2()
{
    std::ifstream infile("PuzzleInputs/03.txt");
    std::string line;
    std::getline(infile, line);

    struct coordinate
    {
        int x;
        int y;
    };

    // current_x[0] corresponds to Santa, current_x[1] corresponds to Robo-Santa. Same for current_y.
    int current_x[2] = {0, 0};
    int current_y[2] = {0, 0};

    std::vector<coordinate> coordinates;
    coordinates.push_back(coordinate{.x = current_x[0], .y = current_y[0]});

    int housesVisited = 1;

    for (int i = 0; i < line.size(); i++)
    {
        // santa_index is 0 for Santa, 1 for Robo-Santa.
        int santa_index = i % 2;

        char instruction = line[i];

        if (instruction == '^')
            current_y[santa_index]++;
        if (instruction == 'v')
            current_y[santa_index]--;
        if (instruction == '>')
            current_x[santa_index]++;
        if (instruction == '<')
            current_x[santa_index]--;

        bool visitedHouse = false;

        for (int j = 0; j < coordinates.size(); j++)
        {
            if (coordinates[j].x == current_x[santa_index] && coordinates[j].y == current_y[santa_index])
            {
                visitedHouse = true;
                break;
            }
        }

        if (!visitedHouse)
        {
            housesVisited++;
            coordinates.push_back(coordinate{.x = current_x[santa_index], .y = current_y[santa_index]});
        }
    }

    return housesVisited;
}