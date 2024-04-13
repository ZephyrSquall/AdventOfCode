#include <iostream>
#include <fstream>
#include <string>
#include <optional>
#include <ranges>
#include <algorithm>
#include <vector>
#include "Day16Solver.h"

struct Sue
{
    int number;
    std::optional<int> children = std::nullopt;
    std::optional<int> cats = std::nullopt;
    std::optional<int> samoyeds = std::nullopt;
    std::optional<int> pomeranians = std::nullopt;
    std::optional<int> akitas = std::nullopt;
    std::optional<int> vizslas = std::nullopt;
    std::optional<int> goldfish = std::nullopt;
    std::optional<int> trees = std::nullopt;
    std::optional<int> cars = std::nullopt;
    std::optional<int> perfumes = std::nullopt;
};

Sue correct_sue = {
    .children = 3,
    .cats = 7,
    .samoyeds = 2,
    .pomeranians = 3,
    .akitas = 0,
    .vizslas = 0,
    .goldfish = 5,
    .trees = 3,
    .cars = 2,
    .perfumes = 1,
};

Sue get_sue(std::string line)
{
    Sue sue;

    // Substring from the 4th character onwards removes the "Sue ".
    line = line.substr(4);

    // Find how many characters makes up the digit.
    int i = 0;
    while (isdigit(line[i]))
    {
        i++;
    }

    // Get a new substring for the number, then use substrings to cut the number (add 2 to i to also
    // cut the ": ") from line.
    std::string number = line.substr(0, i);
    sue.number = stoi(number);
    line = line.substr(i + 2);

    auto things_and_quantities = line | std::ranges::views::split(std::string_view{", "});
    for (const auto &thing_and_quantity : things_and_quantities)
    {
        std::string thing;

        auto split_thing_and_quantity = thing_and_quantity | std::ranges::views::split(std::string_view{": "});
        for (const auto &&[index, thing_or_quality] : std::views::enumerate(split_thing_and_quantity))
        {

            // Convert view to string.
            std::string thing_or_quality_str;
            std::ranges::copy(thing_or_quality | std::ranges::views::transform([](char c)
                                                                               { return (char)c; }),
                              std::back_inserter(thing_or_quality_str));

            if (index == 0)
            {
                // store the thing for next loop when the number is obtained.
                thing = thing_or_quality_str;
            }
            else
            {
                if (thing == "children")
                {
                    sue.children = stoi(thing_or_quality_str);
                }
                else if (thing == "cats")
                {
                    sue.cats = stoi(thing_or_quality_str);
                }
                else if (thing == "samoyeds")
                {
                    sue.samoyeds = stoi(thing_or_quality_str);
                }
                else if (thing == "pomeranians")
                {
                    sue.pomeranians = stoi(thing_or_quality_str);
                }
                else if (thing == "akitas")
                {
                    sue.akitas = stoi(thing_or_quality_str);
                }
                else if (thing == "vizslas")
                {
                    sue.vizslas = stoi(thing_or_quality_str);
                }
                else if (thing == "goldfish")
                {
                    sue.goldfish = stoi(thing_or_quality_str);
                }
                else if (thing == "trees")
                {
                    sue.trees = stoi(thing_or_quality_str);
                }
                else if (thing == "cars")
                {
                    sue.cars = stoi(thing_or_quality_str);
                }
                else if (thing == "perfumes")
                {
                    sue.perfumes = stoi(thing_or_quality_str);
                }
            }
        }
    }

    return sue;
}

std::string AdventOfCode2015::Day16Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/16.txt");
    std::string line;

    std::vector<Sue> sues = {};

    while (getline(infile, line))
    {
        sues.push_back(get_sue(line));
    }

    int correct_sue_number = 0;

    // Check each Sue. Immediately continue to the next loop as soon as an inconsistent value is
    // found (a value can only be inconsistent if it exists to check has.value() first)
    for (Sue sue : sues)
    {
        if (sue.children.has_value() && sue.children != correct_sue.children)
        {
            continue;
        }
        if (sue.cats.has_value() && sue.cats != correct_sue.cats)
        {
            continue;
        }
        if (sue.samoyeds.has_value() && sue.samoyeds != correct_sue.samoyeds)
        {
            continue;
        }
        if (sue.pomeranians.has_value() && sue.pomeranians != correct_sue.pomeranians)
        {
            continue;
        }
        if (sue.akitas.has_value() && sue.akitas != correct_sue.akitas)
        {
            continue;
        }
        if (sue.vizslas.has_value() && sue.vizslas != correct_sue.vizslas)
        {
            continue;
        }
        if (sue.goldfish.has_value() && sue.goldfish != correct_sue.goldfish)
        {
            continue;
        }
        if (sue.trees.has_value() && sue.trees != correct_sue.trees)
        {
            continue;
        }
        if (sue.cars.has_value() && sue.cars != correct_sue.cars)
        {
            continue;
        }
        if (sue.perfumes.has_value() && sue.perfumes != correct_sue.perfumes)
        {
            continue;
        }

        // If no continue statement has been hit yet, then every value is consistent, so this is the
        // correct Sue.
        correct_sue_number = sue.number;
        break;
    }

    return std::to_string(correct_sue_number);
}

std::string AdventOfCode2015::Day16Solver::SolvePart2()
{
    std::ifstream infile("PuzzleInputs/16.txt");
    std::string line;

    std::vector<Sue> sues = {};

    while (getline(infile, line))
    {
        sues.push_back(get_sue(line));
    }

    int correct_sue_number = 0;

    // Check each Sue. Immediately continue to the next loop as soon as an inconsistent value is
    // found (a value can only be inconsistent if it exists to check has.value() first)
    for (Sue sue : sues)
    {
        if (sue.children.has_value() && sue.children != correct_sue.children)
        {
            continue;
        }
        // The correct sue has greater than the indicated number of cats, so the value is
        // inconsistent if it is smaller or equal to it.
        if (sue.cats.has_value() && sue.cats <= correct_sue.cats)
        {
            continue;
        }
        if (sue.samoyeds.has_value() && sue.samoyeds != correct_sue.samoyeds)
        {
            continue;
        }
        // The correct sue has less than the indicated number of pomeranians, so the value is
        // inconsistent if it is greater or equal to it.
        if (sue.pomeranians.has_value() && sue.pomeranians >= correct_sue.pomeranians)
        {
            continue;
        }
        if (sue.akitas.has_value() && sue.akitas != correct_sue.akitas)
        {
            continue;
        }
        if (sue.vizslas.has_value() && sue.vizslas != correct_sue.vizslas)
        {
            continue;
        }
        // The correct sue has less than the indicated number of goldfish, so the value is
        // inconsistent if it is greater or equal to it.
        if (sue.goldfish.has_value() && sue.goldfish >= correct_sue.goldfish)
        {
            continue;
        }
        // The correct sue has greater than the indicated number of trees, so the value is
        // inconsistent if it is smaller or equal to it.
        if (sue.trees.has_value() && sue.trees <= correct_sue.trees)
        {
            continue;
        }
        if (sue.cars.has_value() && sue.cars != correct_sue.cars)
        {
            continue;
        }
        if (sue.perfumes.has_value() && sue.perfumes != correct_sue.perfumes)
        {
            continue;
        }

        // If no continue statement has been hit yet, then every value is consistent, so this is the
        // correct Sue.
        correct_sue_number = sue.number;
        break;
    }

    return std::to_string(correct_sue_number);
}