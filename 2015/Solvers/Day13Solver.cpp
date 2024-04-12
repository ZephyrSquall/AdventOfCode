#include <iostream>
#include <fstream>
#include <string>
#include <ranges>
#include <algorithm>
#include <vector>
#include <functional>
#include "Day13Solver.h"

struct Relationship
{
    std::string attendee;
    std::string neighbour;
    int happiness_modifier;
};

Relationship get_relationship(std::string line)
{
    Relationship relationship;

    auto attendee_and_rest = line | std::ranges::views::split(std::string_view{" would "});
    for (const auto &&[index, attendee_or_rest] : std::views::enumerate(attendee_and_rest))
    {
        // Convert view to string.
        std::string attendee_or_rest_str;
        std::ranges::copy(attendee_or_rest | std::ranges::views::transform([](char c)
                                                                           { return (char)c; }),
                          std::back_inserter(attendee_or_rest_str));

        // attendee
        if (index == 0)
        {
            relationship.attendee = attendee_or_rest_str;
        }
        // rest
        else
        {
            auto happiness_modifier_and_neighbour = attendee_or_rest | std::ranges::views::split(std::string_view{" happiness units by sitting next to "});
            for (const auto &&[inner_index, happiness_modifier_or_neighbour] : std::views::enumerate(happiness_modifier_and_neighbour))
            {
                // Convert view to string.
                std::string happiness_modifier_or_neighbour_str;
                std::ranges::copy(happiness_modifier_or_neighbour | std::ranges::views::transform([](char c)
                                                                                                  { return (char)c; }),
                                  std::back_inserter(happiness_modifier_or_neighbour_str));

                // happiness_modifier
                if (inner_index == 0)
                {
                    // "gain" and "lose" are both 4 letters long so can be obtained with substrings.
                    std::string action = happiness_modifier_or_neighbour_str.substr(0, 4);
                    std::string number = happiness_modifier_or_neighbour_str.substr(5);

                    int happiness_modifier = stoi(number);
                    if (action == "lose")
                    {
                        happiness_modifier *= -1;
                    }

                    relationship.happiness_modifier = happiness_modifier;
                }
                // neighbour
                else
                {
                    // The full stop can be easily removed by popping the string.
                    happiness_modifier_or_neighbour_str.pop_back();
                    relationship.neighbour = happiness_modifier_or_neighbour_str;
                }
            }
        }
    }

    return relationship;
}

std::string AdventOfCode2015::Day13Solver::SolvePart1()
{
    std::ifstream infile("PuzzleInputs/13.txt");
    std::string line;

    std::vector<std::string> attendees;
    std::vector<Relationship> relationships;

    while (getline(infile, line))
    {
        Relationship relationship = get_relationship(line);

        if (std::count(attendees.begin(), attendees.end(), relationship.attendee) == 0)
        {
            attendees.push_back(relationship.attendee);
        }

        relationships.push_back(relationship);
    }

    int maximum_happiness = INT_MIN;
    int attendees_size = attendees.size();

    std::function<int(std::string, std::string)> get_happiness_modifier = [relationships](std::string attendee, std::string neighbour)
    {
        for (Relationship relationship : relationships)
        {
            if (relationship.attendee == attendee && relationship.neighbour == neighbour)
            {
                return relationship.happiness_modifier;
            }
        }
        // This exception should never happen. It's here to prevent compiler warnings about not all
        // code paths returning ints.
        throw std::invalid_argument("attendee and neighbour have no relationship defined");
    };

    std::function<void(std::vector<std::string>)> check_permutation_happiness = [get_happiness_modifier, attendees_size, &maximum_happiness](std::vector<std::string> attendee_permutation)
    {
        int happiness_total = 0;
        // Handle the first and last attendee separately, as their indexes must be manually adjusted to wrap around to the other end of the vector (since the table is a circle).
        happiness_total += get_happiness_modifier(attendee_permutation[0], attendee_permutation[attendees_size - 1]);
        happiness_total += get_happiness_modifier(attendee_permutation[0], attendee_permutation[1]);
        happiness_total += get_happiness_modifier(attendee_permutation[attendees_size - 1], attendee_permutation[attendees_size-2]);
        happiness_total += get_happiness_modifier(attendee_permutation[attendees_size - 1], attendee_permutation[0]);

        for(int i = 1; i < attendees_size - 1; i++)
        {
            happiness_total += get_happiness_modifier(attendee_permutation[i], attendee_permutation[i-1]);
            happiness_total += get_happiness_modifier(attendee_permutation[i], attendee_permutation[i+1]);
        }

        if (happiness_total > maximum_happiness)
        {
            maximum_happiness = happiness_total;
        }
    };

    // Use Heap's algorithm to iterate over every permutation of attendees. On each iteration, calculate the total happiness of all attendees and record it if it's the highest total happiness found.
    std::function<void(int k, std::vector<std::string> &)> generate = [check_permutation_happiness, &generate](int k, std::vector<std::string> &A)
    {
        if (k == 1)
        {
            check_permutation_happiness(A);
        }
        else
        {
            generate(k - 1, A);
            for (int i = 0; i < k - 1; i++)
            {
                // If k is even
                if (k % 2 == 0)
                {
                    // Swap A[i] and A[k-1]
                    std::string temp = A[k - 1];
                    A[k - 1] = A[i];
                    A[i] = temp;
                }
                // If k is odd
                else
                {
                    // Swap A[0] and A[k-1]
                    std::string temp = A[k - 1];
                    A[k - 1] = A[0];
                    A[0] = temp;
                }
                generate(k - 1, A);
            }
        }
    };

    generate(attendees_size, attendees);

    return std::to_string(maximum_happiness);
}

std::string AdventOfCode2015::Day13Solver::SolvePart2()
{
    return "0";
}