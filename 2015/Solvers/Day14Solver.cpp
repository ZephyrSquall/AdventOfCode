#include <iostream>
#include <fstream>
#include <string>
#include <ranges>
#include <algorithm>
#include <vector>
#include "Day14Solver.h"

struct Reindeer
{
    std::string name;
    int speed;
    int flying_time;
    int resting_time;
    int distance = 0;
    int state_timer = 0;
    bool is_resting = false;
};

Reindeer get_reindeer(std::string line)
{
    Reindeer reindeer;

    auto name_and_rest = line | std::ranges::views::split(std::string_view{" can fly "});
    for (const auto &&[index, name_or_rest] : std::views::enumerate(name_and_rest))
    {
        // name
        if (index == 0)
        {
            // Convert view to string.
            std::string name;
            std::ranges::copy(name_or_rest | std::ranges::views::transform([](char c)
                                                                           { return (char)c; }),
                              std::back_inserter(name));

            reindeer.name = name;
        }
        // rest
        else
        {
            auto speed_and_rest = name_or_rest | std::ranges::views::split(std::string_view{" km/s for "});
            for (const auto &&[index, speed_or_rest] : std::views::enumerate(speed_and_rest))
            {
                // speed
                if (index == 0)
                {
                    // Convert view to string.
                    std::string speed;
                    std::ranges::copy(speed_or_rest | std::ranges::views::transform([](char c)
                                                                                    { return (char)c; }),
                                      std::back_inserter(speed));

                    reindeer.speed = stoi(speed);
                }
                // rest
                else
                {
                    auto flying_time_and_resting_time = speed_or_rest | std::ranges::views::split(std::string_view{" seconds, but then must rest for "});
                    for (const auto &&[index, flying_time_or_resting_time] : std::views::enumerate(flying_time_and_resting_time))
                    {
                        // Convert view to string.
                        std::string flying_time_or_resting_time_str = "";
                        std::ranges::copy(flying_time_or_resting_time | std::ranges::views::transform([](char c)
                                                                                                      { return (char)c; }),
                                          std::back_inserter(flying_time_or_resting_time_str));

                        // flying_time
                        if (index == 0)
                        {
                            reindeer.flying_time = stoi(flying_time_or_resting_time_str);
                        }
                        // resting_time
                        else
                        {
                            // Remove 9 characters from end of string to remove " seconds."
                            flying_time_or_resting_time_str.erase(flying_time_or_resting_time_str.size() - 9);
                            reindeer.resting_time = stoi(flying_time_or_resting_time_str);
                        }
                    }
                }
            }
        }
    }

    return reindeer;
}

std::string AdventOfCode2015::Day14Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/14.txt");
    std::string line;

    std::vector<Reindeer> reindeers = {};

    while (getline(infile, line))
    {
        reindeers.push_back(get_reindeer(line));
    }

    const int race_time = 2503;

    for (int i = 0; i < race_time; i++)
    {
        for (Reindeer &reindeer : reindeers)
        {
            reindeer.state_timer++;
            if (reindeer.is_resting)
            {
                if(reindeer.state_timer == reindeer.resting_time)
                {
                    reindeer.is_resting = false;
                    reindeer.state_timer = 0;
                }
            }
            else
            {
                reindeer.distance += reindeer.speed;

                if(reindeer.state_timer == reindeer.flying_time)
                {
                    reindeer.is_resting = true;
                    reindeer.state_timer = 0;
                }
            }
        }
    }

    int max_distance = 0;
    
    for (Reindeer reindeer : reindeers)
    {
        if (reindeer.distance > max_distance)
        {
            max_distance = reindeer.distance;
        }
    }  

    return std::to_string(max_distance);
}

std::string AdventOfCode2015::Day14Solver::SolvePart2()
{
    return "0";
}